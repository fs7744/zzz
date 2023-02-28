namespace Zzz.Hosting
{
    public interface IHost : IAsyncDisposable
    {
        IServiceProvider Services { get; }

        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}