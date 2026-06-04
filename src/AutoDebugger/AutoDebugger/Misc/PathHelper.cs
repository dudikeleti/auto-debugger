using System.Runtime.CompilerServices;

namespace AutoDebugger.Misc
{
    internal static class PathHelper
    {
        private const string RelativePath = "Misc\\" + nameof(PathHelper) + ".cs";
        private static string? _projectPath;
        internal static string ProjectPath
        {
            get { return _projectPath ??= GetPath(); }
        }

        private static string GetPath()
        {
            string pathName = GetSourceFilePath();
            return pathName[..^RelativePath.Length];
        }

        internal static string GetSourceFilePath([CallerFilePath] string? callerFilePath = null) => callerFilePath ?? "";

        internal static string GetPathForYamlFile(string fileName)
        {
            return Path.Combine(ProjectPath, "Plugins", "Yaml", fileName);
        }

        internal static string GetPathForSymbolFile(string fileName)
        {
            return Path.Combine(ProjectPath, "Symbols", fileName);
        }
    }
}
