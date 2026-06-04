using Microsoft.SemanticKernel;
using System.ComponentModel;
using AutoDebugger.Probes;

namespace AutoDebugger.Plugins
{
    internal class SendLogPlugin : DiagnosticPlugin
    {
        [KernelFunction, Description("Add a log and return response that contains id of the log")]
        public string SendLog(
            [Description("A json text that represnt the logs to define")] string logJsonToAdd)
        {
            try
            {
                var logSender = new LogSenderFileSystem(logJsonToAdd);
                var ids = logSender.SendLog().GetAwaiter().GetResult();
                CaptureCall(nameof(SendLog),
                    ids,
                    new Dictionary<string, object?>
                    {
                        {nameof(logJsonToAdd), logJsonToAdd}
                    });

                return ids;
            }
            catch (Exception e)
            {
                var errorReturn = $"There was error when trying to add log. Error: {e.Message}";
                CaptureCall(nameof(SendLog),
                    errorReturn,
                new Dictionary<string, object?>
                {
                        {nameof(logJsonToAdd), logJsonToAdd}
                });

                return errorReturn;
            }
        }
    }
}
