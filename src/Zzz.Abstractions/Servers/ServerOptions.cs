namespace Zzz.Abstractions.Servers
{
    public class ServerOptions
    {
        public long? MaxConcurrentUpgradedConnections { get; set; }

        public List<ListenOptions> ListenOptions { get; set; }
    }
}