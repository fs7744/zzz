using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zzz.Host;

namespace TcpDemo
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<HostedService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}