using System.Net;

namespace Zzz.Connections
{
    public interface IConnectionListenerFactorySelector
    {
        bool CanBind(EndPoint endpoint);
    }
}