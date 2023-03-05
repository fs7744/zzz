using System.Net;
using Zzz.Connections;

namespace Zzz.Servers
{
    public class ListenOptionsBuilder
    {
        private readonly EndPoint[] endPoints;
        public List<Func<ConnectionDelegate, ConnectionDelegate>> Middlewares { get; } = new List<Func<ConnectionDelegate, ConnectionDelegate>>();
        public IServiceProvider ServiceProvider { get; private set; }

        public ListenOptionsBuilder(EndPoint[] endPoints)
        {
            this.endPoints = endPoints;
        }

        internal ListenOptions Build(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ConnectionDelegate app = context =>
            {
                return Task.CompletedTask;
            };

            foreach (var component in Middlewares.Reverse<Func<ConnectionDelegate, ConnectionDelegate>>())
            {
                app = component(app);
            }

            return new ListenOptions() { EndPoints = endPoints, ConnectionDelegate = app };
        }
    }
}