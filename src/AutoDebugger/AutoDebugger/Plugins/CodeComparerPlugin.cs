using System.ComponentModel;
using AutoDebugger.Misc;
using Microsoft.SemanticKernel;

namespace AutoDebugger.Plugins
{
    internal class CodeComparerPlugin : DiagnosticPlugin
    {
        [KernelFunction, Description("Compare two pieces of code")]
        public string CompareFiles(
                [Description("The original code before changes")] string originalCode,
                [Description("The new code after changed")] string newCode)
        {
            var originalCodeToTrace = originalCode?.Length > 100 ? originalCode[..99] + " ..." : originalCode;
            var newCodeToTrace = newCode?.Length > 100 ? newCode[..99] + " ..." : newCode;

            if (!Validate(originalCode, newCode, out var error))
            {
                CaptureCall(nameof(CompareFiles),
                    error,
                    new Dictionary<string, object?>
                    {
                        {nameof(originalCode), originalCodeToTrace},
                        {nameof(newCode), newCodeToTrace}
                    });

                return error;
            }

            (string originalFile, string newFile) filesToCompare;
            try
            {
                filesToCompare = GetFilesToCompare(originalCode!, newCode!);
            }
            catch (Exception e)
            {
                CaptureCall(nameof(CompareFiles),
                    e.Message,
                    new Dictionary<string, object?>
                    {
                        {nameof(originalCode), originalCodeToTrace},
                        {nameof(newCode), newCodeToTrace}
                    });

                return e.Message;
            }

            var startInfo =
                new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Config.BeyondComparePath,
                    Arguments = $"{filesToCompare.originalFile} {filesToCompare.newFile}"
                };

            string result;
            var p = System.Diagnostics.Process.Start(startInfo);
            if (p == null)
            {
                result = $"Error to start process {startInfo.FileName}";
            }
            else
            {
                p.WaitForExit();
                try
                {
                    var exitCode = p.ExitCode;
                    result = exitCode == 0 ? "Process ended successfully" : $"Process ended with error: {p.ExitCode}";

                }
                catch (Exception e)
                {
                    result = e.Message;
                }
            }

            CaptureCall(nameof(CompareFiles),
                result,
                new Dictionary<string, object?>
                {
                    {nameof(originalCode), originalCodeToTrace},
                    {nameof(newCode), newCodeToTrace}
                });

            return result;
        }

        private static bool Validate(string? originalCode, string? newCode, out string error)
        {
            if (string.IsNullOrEmpty(originalCode) || string.IsNullOrEmpty(newCode))
            {
                error = "Invalid arguments. Provided code must be non empty.";
                return false;
            }

            if (!File.Exists(Config.BeyondComparePath))
            {
                error = "Compare tool exe does not exist.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private static (string originalFile, string newFile) GetFilesToCompare(string originalCode, string newCode)
        {
            var guid = Guid.NewGuid();
            var originalCodePath = Path.Combine(Path.GetTempPath(), "original_" + guid + ".txt");
            var newCodePath = Path.Combine(Path.GetTempPath(), "new_" + guid + ".txt");

            File.WriteAllText(originalCodePath, originalCode);
            File.WriteAllText(newCodePath, newCode);

            return (originalCodePath, newCodePath);
        }
    }
}
