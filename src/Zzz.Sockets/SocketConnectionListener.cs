using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Zzz.Connections;
using Zzz.Exceptions;
using Zzz.Sockets.Internal;

namespace Zzz.Sockets
{
    public sealed class SocketConnectionListener : IConnectionListener
    {
        private readonly SocketConnectionContextFactory _factory;
        private readonly ILogger _logger;
        private Socket? _listenSocket;
        private readonly SocketTransportOptions _options;

        public EndPoint EndPoint { get; private set; }

        internal SocketConnectionListener(
            EndPoint endpoint,
            SocketTransportOptions options,
            ILoggerFactory loggerFactory)
        {
            EndPoint = endpoint;
            _options = options;
            var logger = loggerFactory.CreateLogger<SocketConnectionListener>();
            _logger = logger;
            _factory = new SocketConnectionContextFactory(new SocketConnectionFactoryOptions(options), logger);
        }

        internal void Bind()
        {
            if (_listenSocket != null)
            {
                throw new InvalidOperationException("Transport is already bound.");
            }

            Socket listenSocket;
            try
            {
                listenSocket = _options.CreateBoundListenSocket(EndPoint);
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
            {
                throw new AddressInUseException(e.Message, e);
            }

            Debug.Assert(listenSocket.LocalEndPoint != null);
            EndPoint = listenSocket.LocalEndPoint;

            listenSocket.Listen(_options.Backlog);

            _listenSocket = listenSocket;
        }

        public async ValueTask<ConnectionContext?> AcceptAsync(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                try
                {
                    Debug.Assert(_listenSocket != null, "Bind must be called first.");

                    var acceptSocket = await _listenSocket.AcceptAsync(cancellationToken);

                    // Only apply no delay to Tcp based endpoints
                    if (acceptSocket.LocalEndPoint is IPEndPoint)
                    {
                        acceptSocket.NoDelay = _options.NoDelay;
                    }

                    return _factory.Create(acceptSocket);
                }
                catch (ObjectDisposedException)
                {
                    // A call was made to UnbindAsync/DisposeAsync just return null which signals we're done
                    return null;
                }
                catch (SocketException e) when (e.SocketErrorCode == SocketError.OperationAborted)
                {
                    // A call was made to UnbindAsync/DisposeAsync just return null which signals we're done
                    return null;
                }
                catch (SocketException)
                {
                    // The connection got reset while it was in the backlog, so we try again.
                    SocketsLog.ConnectionReset(_logger, connectionId: "(null)");
                }
            }
        }

        public ValueTask UnbindAsync(CancellationToken cancellationToken = default)
        {
            _listenSocket?.Dispose();
            return default;
        }

        public ValueTask DisposeAsync()
        {
            _listenSocket?.Dispose();

            _factory.Dispose();

            return default;
        }
    }
}