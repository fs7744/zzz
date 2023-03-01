using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzz.Servers;

namespace Zzz
{
    public class HostedService : IHostedService, IAsyncDisposable
    {
        private readonly IServer server;

        public HostedService(IServer server)
        {
            this.server = server;
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync().ConfigureAwait(false);
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return server.StartAsync(cancellationToken);
        }
           
        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return server.StopAsync(cancellationToken);
        }
    }
}
