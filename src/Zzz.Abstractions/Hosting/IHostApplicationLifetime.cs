namespace Zzz.Hosting
{
    public interface IHostApplicationLifetime
    {
        /// <summary>
        /// Triggered when the application host has fully started.
        /// </summary>
        CancellationToken ApplicationStarted { get; }

        /// <summary>
        /// Triggered when the application host is starting a graceful shutdown.
        /// Shutdown will block until all callbacks registered on this token have completed.
        /// </summary>
        CancellationToken ApplicationStopping { get; }

        /// <summary>
        /// Triggered when the application host has completed a graceful shutdown.
        /// The application will not exit until all callbacks registered on this token have completed.
        /// </summary>
        CancellationToken ApplicationStopped { get; }

        /// <summary>
        /// Requests termination of the current application.
        /// </summary>
        void StopApplication();
    }
}