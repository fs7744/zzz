using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzz.Abstractions.Servers;
using Zzz.Connections;
using Zzz.Features;
using Zzz.Servers;

namespace Zzz.Core.Servers
{
    public class ZzzServer : IServer
    {
        private bool _hasStarted;
        private int _stopping;
        private readonly SemaphoreSlim _bindSemaphore = new SemaphoreSlim(initialCount: 1);
        private readonly CancellationTokenSource _stopCts = new CancellationTokenSource();
        private readonly TaskCompletionSource _stoppedTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly ServiceContext _serviceContext;

        public ZzzServer(ServerOptions serverOptions, ILoggerFactory loggerFactory, DiagnosticSource? diagnosticSource)
        {
            var trace = new ZzzTrace(loggerFactory);
            var connectionManager = new ConnectionManager(
                trace,
                serverOptions.MaxConcurrentUpgradedConnections);

            var heartbeatManager = new HeartbeatManager(connectionManager);

            var heartbeat = new Heartbeat(
                new IHeartbeatHandler[] { heartbeatManager },
                new SystemClock(),
                trace);
            _serviceContext = new ServiceContext
            {
                Log = trace,
                Scheduler = PipeScheduler.ThreadPool,
                SystemClock = heartbeatManager,
                ConnectionManager = connectionManager,
                Heartbeat = heartbeat,
                ServerOptions = serverOptions,
                DiagnosticSource = diagnosticSource
            };
        }


        public async ValueTask DisposeAsync()
        {
            await StopAsync(new CancellationToken(canceled: true)).ConfigureAwait(false);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_hasStarted)
                {
                    throw new InvalidOperationException("Server already started");
                }
                _hasStarted = true;
                
                _serviceContext.Heartbeat?.Start();

                await BindAsync(cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                await DisposeAsync();
                throw;
            }
        }

        private async Task BindAsync(CancellationToken cancellationToken)
        {
            await _bindSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                if (_stopping == 1)
                {
                    throw new InvalidOperationException("Kestrel has already been stopped.");
                }

                //IChangeToken? reloadToken = null;

                //_serverAddresses.InternalCollection.PreventPublicMutation();

                //if (Options.ConfigurationLoader?.ReloadOnChange == true && (!_serverAddresses.PreferHostingUrls || _serverAddresses.InternalCollection.Count == 0))
                //{
                //    reloadToken = Options.ConfigurationLoader.Configuration.GetReloadToken();
                //}

                //Options.ConfigurationLoader?.Load();

                //await AddressBinder.BindAsync(Options.ListenOptions, AddressBindContext!, cancellationToken).ConfigureAwait(false);
                //_configChangedRegistration = reloadToken?.RegisterChangeCallback(TriggerRebind, this);
            }
            finally
            {
                _bindSemaphore.Release();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (Interlocked.Exchange(ref _stopping, 1) == 1)
            {
                await _stoppedTcs.Task.ConfigureAwait(false);
                return;
            }

            _stopCts.Cancel();

#pragma warning disable CA2016 // Don't use cancellationToken when acquiring the semaphore. Dispose calls this with a pre-canceled token.
            await _bindSemaphore.WaitAsync().ConfigureAwait(false);
#pragma warning restore CA2016

            try
            {
                //await _transportManager.StopAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _stoppedTcs.TrySetException(ex);
                throw;
            }
            finally
            {
                //ServiceContext.Heartbeat?.Dispose();
                //_configChangedRegistration?.Dispose();
                _stopCts.Dispose();
                _bindSemaphore.Release();
            }

            _stoppedTcs.TrySetResult();
        }
    }
}
