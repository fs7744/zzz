using Zzz.Features;

namespace Zzz.Servers
{
    public interface IServer : IAsyncDisposable
    {
        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}