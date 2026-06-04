namespace AutoDebugger.Plugins
{
    internal abstract class DiagnosticPlugin
    {
        internal static Dictionary<string, int> Counts { get; } = [];

        internal static List<Diagnostic> Traces { get; } = [];

        internal static void Reset()
        {
            Counts.Clear();
            Traces.Clear();
        }

        protected T CaptureCall<T>(
            string methodName,
            T returnValue,
            Dictionary<string, object?>? parameters = null)
        {
            var typeName = GetType().Name;

            Console.WriteLine();
            Console.WriteLine($"# {typeName}.{methodName}");

            Counts.TryGetValue($"{typeName}.{methodName}", out var count);
            Counts[$"{typeName}.{methodName}"] = ++count;

            Traces.Add(new Diagnostic($"{typeName}.{methodName}", parameters, returnValue));

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    Console.WriteLine($"  - {parameter.Key}:{parameter.Value}");
                }
            }

            Console.WriteLine($"  > {returnValue}");
            Console.WriteLine();

            return returnValue;
        }

        internal class Diagnostic(string function, Dictionary<string, object?>? parameters, object? returnValue)
        {
            internal string Function { get; } = function;

            internal Dictionary<string, object?> Parameters = parameters ?? [];

            internal Type Type => Value?.GetType() ?? typeof(object);

            internal object? Value { get; } = returnValue;
        }
    }
}
