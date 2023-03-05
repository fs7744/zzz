using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Zzz.Connections;

namespace Zzz.Servers
{
    public static class ServerOptionsExtensions
    {
        public static ListenOptionsBuilder AddEndPoints(this ServerOptionsBuilder builder, params EndPoint[] endPoints)
        {
            var lo = new ListenOptionsBuilder(endPoints);
            builder.ListenOptionsBuilders.Add(lo);
            return lo;
        }

        public static ListenOptionsBuilder UseMiddleware(this ListenOptionsBuilder builder, Func<ConnectionDelegate, ConnectionDelegate> middleware)
        {
            builder.Middlewares.Add(middleware);
            return builder;
        }

        public static ListenOptionsBuilder UseMiddleware<T>(this ListenOptionsBuilder builder) where T : IMiddleware
        {
            builder.UseMiddleware(next =>
            {
                var serviceProvider = builder.ServiceProvider;
                var p = serviceProvider.GetRequiredService<T>();
                return c => p.Invoke(c, next);
            });
            return builder;
        }
    }
}