using System.Net;

namespace Zzz.Connections
{
    public interface IConnectionListenerFactory
    {
        ValueTask<IConnectionListener> BindAsync(EndPoint endpoint, CancellationToken cancellationToken = default);
    }
}