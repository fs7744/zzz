using Microsoft.Extensions.DependencyInjection;
using Zzz;
using Zzz.Abstractions.Servers;
using Zzz.Connections;
using Zzz.Core.Servers;
using Zzz.Servers;
using Zzz.Sockets;

namespace Microsoft.Extensions.Hosting
{
    public static class HostExtensions
    {
        public static IHostBuilder ConfigureZzz(this IHostBuilder hostBuilder, Action<ServerOptions> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
             {
                 ServerOptions serverOptions = new();
                 action?.Invoke(serverOptions);
                 services.AddSingleton(serverOptions);
                 services.PostConfigure<SocketTransportOptions>(opt => { });
                 services.AddSingleton<IConnectionFactory, SocketConnectionFactory>();
                 services.AddSingleton<IConnectionListenerFactory, SocketTransportFactory>();
                 services.AddSingleton<IServer, ZzzServer>();
                 services.AddHostedService<HostedService>();
             });
            return hostBuilder;
        }
    }
}