using Microsoft.Extensions.DependencyInjection;
using Zzz;
using Zzz.Connections;
using Zzz.Core.Servers;
using Zzz.Servers;
using Zzz.Sockets;

namespace Microsoft.Extensions.Hosting
{
    public static class HostExtensions
    {
        public static IHostBuilder ConfigureZzz(this IHostBuilder hostBuilder, Action<SocketTransportOptions> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
             {
                 action ??= i => { };
                 services.PostConfigure(action);
                 services.AddSingleton<IConnectionFactory, SocketConnectionFactory>();
                 services.AddSingleton<IConnectionListenerFactory, SocketTransportFactory>();
                 services.AddSingleton<IServer, ZzzServer>();
                 services.AddSingleton<IServer, ZzzServer>();
                 services.AddHostedService<HostedService>();
             });
            return hostBuilder;
        }
    }
}