namespace Zzz.Core.Servers
{
    internal interface IHeartbeatHandler
    {
        void OnHeartbeat(DateTimeOffset now);
    }
}