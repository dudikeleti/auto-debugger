using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text;
using AutoDebugger.Probes;

namespace AutoDebugger.Plugins
{
    internal class GetLogResultPlugin : DiagnosticPlugin
    {
        [KernelFunction, Description("Get result snapshot of the specified logs")]
        public string GetLogResult(
            [Description("Array of ids that represent logs ids")] string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                var errorReturn = "Log id is missing. Check 'ids' argument";
                CaptureCall(nameof(GetLogResult),
                    errorReturn,
                    new Dictionary<string, object?>
                    {
                        {nameof(ids), ids}
                    });

                return errorReturn;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var id in ids.Split(','))
            {
                try
                {
                    using var resultProvider = new LogResultFileSystem(id);
                    var snapshot = resultProvider.GetLogResult().GetAwaiter().GetResult();

                    if (string.IsNullOrEmpty(snapshot))
                    {
                        sb.AppendLine($"Snapshot for id {id} is null");
                    }
                    else
                    {
                        sb.AppendLine(snapshot);
                    }
                }
                catch (Exception e)
                {
                    var errorReturn = $"There was error when trying to get log result for id: {id}. Error: {e.Message}";
                    sb.AppendLine(errorReturn);
                }
            }

            var result = sb.ToString();
            CaptureCall(nameof(GetLogResult),
                result,
                new Dictionary<string, object?>
                {
                    {nameof(ids), ids}
                });

            return result;
        }
    }
}
