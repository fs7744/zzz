using Zzz.Features;

namespace Zzz.Servers
{
    public interface IServer : IDisposable
    {
        IFeatureCollection Features { get; }

        Task StartAsync<TContext>(CancellationToken cancellationToken) where TContext : notnull;

        Task StopAsync(CancellationToken cancellationToken);
    }
}