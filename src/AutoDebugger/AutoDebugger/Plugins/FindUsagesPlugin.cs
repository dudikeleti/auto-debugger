using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text;

namespace AutoDebugger.Plugins
{
    internal class FindUsagesPlugin : DiagnosticPlugin
    {
        [KernelFunction, Description("Find usages of specific method. i.e. who call this method")]
        public string FindUsages(
            [Description("Full path of the source file of the class where the method we want to find usages exists")] string classPath,
            [Description("Method that we want to find usages for")] string methodName)
        {
            try
            {
                var directory = Directory.GetParent(classPath)?.Parent;
                if (directory is not { Exists: true })
                {
                    return "Path of the class does not exist, check 'classPath' argument";
                }

                StringBuilder sb = new StringBuilder();
                foreach (var csFile in directory.EnumerateFiles("*.cs", SearchOption.AllDirectories))
                {
                    var current = File.ReadAllText(csFile.FullName);
                    if (current.Contains(methodName + "("))
                    {
                        sb.AppendLine(csFile.FullName);
                    }
                }

                var found = sb.ToString();
                if (string.IsNullOrWhiteSpace(found))
                {
                    return "You are probably sent invalid or partial path to the plugin so I can't found usages";
                }

                CaptureCall(nameof(FindUsages),
                    found,
                    new Dictionary<string, object?>
                    {
                        {nameof(classPath), classPath},
                        {nameof(methodName), methodName}
                    });

                return found;
            }
            catch (Exception e)
            {
                var errorReturn = $"There was an error in getting the usages of the method. Verify the 'classPath' argument. Error: {e.Message}";
                CaptureCall(nameof(FindUsages),
                    errorReturn,
                    new Dictionary<string, object?>
                    {
                        {nameof(classPath), classPath},
                        {nameof(methodName), methodName}
                    });

                return errorReturn;
            }
        }
    }
}
