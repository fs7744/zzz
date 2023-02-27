using Microsoft.Extensions.Hosting;

namespace Zzz.Host
{
    public class HostedService : IHostedService, IAsyncDisposable
    {
        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}