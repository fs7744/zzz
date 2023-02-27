using System.Net;

namespace Zzz.Connections
{
    public interface IConnectionFactory
    {
        ValueTask<ConnectionContext> ConnectAsync(EndPoint endpoint, CancellationToken cancellationToken = default);
    }
}