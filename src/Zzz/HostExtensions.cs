using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zzz;
using Zzz.Connections;
using Zzz.Core.Servers;
using Zzz.Servers;
using Zzz.Sockets;

namespace Microsoft.Extensions.Hosting
{
    public static class HostExtensions
    {
        public static IHostBuilder ConfigureZzz(this IHostBuilder hostBuilder, Action<ServerOptionsBuilder> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
             {
                 ServerOptionsBuilder serverOptions = new(services);
                 action?.Invoke(serverOptions);
                 services.AddSingleton((serviceProvider) => serverOptions.Build(serviceProvider));
                 services.PostConfigure<SocketTransportOptions>(opt => { });
                 services.TryAddSingleton<IConnectionFactory, SocketConnectionFactory>();
                 services.TryAddSingleton<IConnectionListenerFactory, SocketTransportFactory>();
                 services.TryAddSingleton<IServer, ZzzServer>();
                 services.AddHostedService<HostedService>();
             });
            return hostBuilder;
        }
    }
}