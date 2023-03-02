using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TcpDemo
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureZzz(i => 
                {
                    
                })
                .Build();

            await host.RunAsync();
        }
    }
}