using Zzz.Connections;

namespace Zzz.Servers
{
    public interface IMiddleware
    {
        Task Invoke(ConnectionContext connection, ConnectionDelegate next);
    }
}