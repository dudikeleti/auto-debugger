using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AutoDebugger.Plugins
{
    internal class WaitPlugin : DiagnosticPlugin
    {
        /// <summary>
        /// Wait a given amount of seconds
        /// </summary>
        [KernelFunction, Description("Wait a given amount of seconds")]
        public async Task SecondsAsync([Description("The number of seconds to wait")] double seconds) =>
            await CaptureCall(nameof(SecondsAsync),
                new Func<Task, Task<string>>(async task =>
                {
                    await task;
                    return "Finish";
                })(Task.Delay(TimeSpan.FromSeconds(seconds))),
                new Dictionary<string, object?>
                {
                    {nameof(seconds), seconds}
                });
    }
}
