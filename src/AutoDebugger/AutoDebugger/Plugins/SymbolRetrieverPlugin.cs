using Microsoft.SemanticKernel;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using AutoDebugger.Misc;

namespace AutoDebugger.Plugins
{
    internal class SymbolRetrieverPlugin : DiagnosticPlugin
    {
        [KernelFunction, Description("Get code symbols for a specific method")]
        public string GetCodeSymbol(
            [Description("Parent class name of the method")] string className,
            [Description("Method name to get symbol for")] string methodName)
        {
            try
            {
                var symbolContent = File.ReadAllText(Config.SymbolFile);
                JObject symbolJson = JObject.Parse(symbolContent);
                var classes = symbolJson["scopes"]?[0]?["scopes"];
                if (classes == null)
                {
                    throw new InvalidOperationException($"Can't find 'classes' symbols for scope {symbolJson["scopes"]?[0] ?? "na"}");
                }

                JToken? foundedClass = null;
                foreach (var @class in classes)
                {
                    if (@class["name"]?.ToString().EndsWith(className) == true)
                    {
                        foundedClass = @class;
                        break;
                    }
                }

                if (foundedClass == null)
                {
                    throw new InvalidOperationException($"Can't find symbols for class {className}");
                }

                JToken? foundedMethod = null;
                var scopes = foundedClass["scopes"];
                if (scopes == null)
                {
                    throw new InvalidOperationException($"Can't find 'scopes' symbols for class {foundedClass}");
                }

                foreach (var method in scopes)
                {
                    if (method["name"]?.ToString() == methodName)
                    {
                        foundedMethod = method;
                    }
                }

                if (foundedMethod == null)
                {
                    throw new InvalidOperationException($"Can't find symbols for method {methodName}");
                }

                CaptureCall(nameof(GetCodeSymbol),
                    foundedMethod.ToString(),
                    new Dictionary<string, object?>
                    {
                        {nameof(className), className},
                        {nameof(methodName), methodName}
                    });

                return foundedMethod.ToString();
            }
            catch (Exception e)
            {
                var errorReturn = $"There was error while try to getting code symbol. Error: {e.Message}";

                CaptureCall(nameof(GetCodeSymbol),
                    errorReturn,
                    new Dictionary<string, object?>
                    {
                        {nameof(className), className},
                        {nameof(methodName), methodName}
                    });

                return errorReturn;
            }
        }
    }
}
