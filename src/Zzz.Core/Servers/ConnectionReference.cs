using System.Diagnostics.CodeAnalysis;

namespace Zzz.Core.Servers
{
    internal sealed class ConnectionReference
    {
        private readonly long _id;
        private readonly WeakReference<ZzzConnection> _weakReference;
        private readonly TransportConnectionManager _transportConnectionManager;

        public ConnectionReference(long id, ZzzConnection connection, TransportConnectionManager transportConnectionManager)
        {
            _id = id;

            _weakReference = new WeakReference<ZzzConnection>(connection);
            ConnectionId = connection.TransportConnection.ConnectionId;

            _transportConnectionManager = transportConnectionManager;
        }

        public string ConnectionId { get; }

        public bool TryGetConnection([NotNullWhen(true)] out ZzzConnection? connection)
        {
            return _weakReference.TryGetTarget(out connection);
        }

        public void StopTrasnsportTracking()
        {
            _transportConnectionManager.StopTracking(_id);
        }
    }
}