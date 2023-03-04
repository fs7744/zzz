using System.Net;
using Zzz.Connections;

namespace Zzz.Abstractions.Servers
{
    public class ListenOptions
    {
        public IReadOnlyCollection<EndPoint> EndPoints { get; set; }

        public ConnectionDelegate ConnectionDelegate { get; set; }
    }
}