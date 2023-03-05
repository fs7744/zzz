using System.Net;
using Zzz.Connections;

namespace Zzz.Servers
{
    public class ListenOptions
    {
        public IReadOnlyCollection<EndPoint> EndPoints { get; set; }

        public ConnectionDelegate ConnectionDelegate { get; set; }
    }
}