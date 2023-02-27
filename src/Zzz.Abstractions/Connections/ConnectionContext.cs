using System.IO.Pipelines;
using System.Net;
using Zzz.Exceptions;
using Zzz.Features;

namespace Zzz.Connections
{
    public abstract class ConnectionContext : IAsyncDisposable
    {
        public abstract IDuplexPipe Transport { get; set; }

        public abstract string ConnectionId { get; set; }
        //public abstract IFeatureCollection Features { get; }

        public abstract IDictionary<object, object?> Items { get; set; }

        public virtual CancellationToken ConnectionClosed { get; set; }
        public virtual EndPoint? LocalEndPoint { get; set; }
        public virtual EndPoint? RemoteEndPoint { get; set; }

        public abstract void Abort(ConnectionAbortedException abortReason);

        public virtual ValueTask DisposeAsync()
        {
            return default;
        }
    }
}