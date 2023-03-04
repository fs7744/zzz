namespace Zzz.Core.Servers
{
    internal sealed class SystemClock : ISystemClock
    {
        /// <summary>
        /// Retrieves the current UTC system time.
        /// </summary>
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        /// <summary>
        /// Retrieves ticks for the current UTC system time.
        /// </summary>
        public long UtcNowTicks => DateTimeOffset.UtcNow.Ticks;

        /// <summary>
        /// Retrieves the current UTC system time.
        /// </summary>
        public DateTimeOffset UtcNowUnsynchronized => DateTimeOffset.UtcNow;
    }
}