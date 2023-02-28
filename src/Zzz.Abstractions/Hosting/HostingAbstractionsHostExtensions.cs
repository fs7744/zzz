using Microsoft.Extensions.DependencyInjection;

namespace Zzz.Hosting
{
    public static class HostingAbstractionsHostExtensions
    {
        public static async Task RunAsync(this IHost host, CancellationToken token = default(CancellationToken))
        {
            try
            {
                await host.StartAsync(token).ConfigureAwait(continueOnCapturedContext: false);
                await host.WaitForShutdownAsync(token).ConfigureAwait(continueOnCapturedContext: false);
            }
            finally
            {
                await host.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public static async Task WaitForShutdownAsync(this IHost host, CancellationToken token = default(CancellationToken))
        {
            IHostApplicationLifetime requiredService = host.Services.GetRequiredService<IHostApplicationLifetime>();
            token.Register(delegate (object state)
            {
                ((IHostApplicationLifetime)state).StopApplication();
            }, requiredService);
            TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            requiredService.ApplicationStopping.Register(delegate (object obj)
            {
                TaskCompletionSource<object> taskCompletionSource2 = (TaskCompletionSource<object>)obj;
                taskCompletionSource2.TrySetResult(null);
            }, taskCompletionSource);
            await taskCompletionSource.Task.ConfigureAwait(continueOnCapturedContext: false);
            await host.StopAsync(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}