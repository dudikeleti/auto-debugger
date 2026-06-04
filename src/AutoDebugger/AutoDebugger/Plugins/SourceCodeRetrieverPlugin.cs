using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AutoDebugger.Plugins
{
    internal class SourceCodeRetrieverPlugin : DiagnosticPlugin
    {
        [KernelFunction, Description("Get the source code of specific line range from a file path")]
        public string GetSource(
            [Description("Full path of the source code class file")] string path,
            [Description("First line to get source code")] int startLine,
            [Description("Last line to get source code")] int endLine)
        {
            if (!Path.Exists(path))
            {
                return $"Path '{path}' does not exist. Verify that 'path' argument is correct.";
            }

            try
            {
                var code = File.ReadAllLines(path);
                startLine = startLine > 3 ? startLine - 3 : startLine;
                var result = string.Join(Environment.NewLine, code.Skip(startLine).Take(endLine - startLine));
                CaptureCall(nameof(GetSource),
                    result,
                    new Dictionary<string, object?>
                    {
                        {nameof(path), path},
                        {nameof(startLine), startLine},
                        {nameof(endLine), endLine}
                    });

                return result;
            }
            catch (Exception e)
            {
                var errorReturn = $"There was error while try to getting source code. Error: {e.Message}";
                CaptureCall(nameof(GetSource),
                    errorReturn,
                    new Dictionary<string, object?>
                    {
                        {nameof(path), path},
                        {nameof(startLine), startLine},
                        {nameof(endLine), endLine}
                    });

                return errorReturn;
            }
        }
    }
}
