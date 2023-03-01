using Zzz.Features;

namespace Zzz.Servers
{
    public interface IServer : IAsyncDisposable
    {
        IFeatureCollection Features { get; }

        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}