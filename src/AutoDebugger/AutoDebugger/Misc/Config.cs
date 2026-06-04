namespace AutoDebugger.Misc
{
    internal static class Config
    {
        private static string? GetUserEnvironmentVariable(string key) => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);

        /// <summary>
        /// Required OpenAI API key.
        /// </summary>
        internal static string ApiKey =>
            GetUserEnvironmentVariable("AD_OPENAI_KEY") ??
            throw new InvalidOperationException("Environment variable 'AD_OPENAI_KEY' undefined.");

        /// <summary>
        /// The model name (defaults to gpt-4-0125-preview).
        /// </summary>
        internal static string ModelName =>
            GetUserEnvironmentVariable("AD_OPENAI_MODEL") ??
            "gpt-4-0125-preview";

        /// <summary>
        /// The output path for writing results (defaults to {Temp}/auto_debugger_results).
        /// </summary>
        internal static string ResultDirectory =>
            GetUserEnvironmentVariable("AD_RESULT_PATH") ??
            Path.Combine(Path.GetTempPath(), "auto_debugger_results");

        /// <summary>
        /// The output path for writing agents results (defaults to {Temp}/auto_debugger_results).
        /// </summary>
        internal static string SymbolFile =>
            GetUserEnvironmentVariable("AD_SYMBOL_PATH") ??
            Path.Combine(PathHelper.GetPathForSymbolFile("WebServiceSymbols.json"));

        /// <summary>
        /// The path for reading snapshots results (defaults to {Temp}/snapshots).
        /// </summary>
        internal static string SnapshotDirectory =>
            GetUserEnvironmentVariable("AD_SNAPSHOT_PATH") ??
            Path.Combine(Path.GetTempPath(), "snapshots");

        /// <summary>
        /// The path for writing new probes (defaults to {Temp}/new_probes).
        /// </summary>
        internal static string NewProbesDirectory =>
            GetUserEnvironmentVariable("AD_PROBES_PATH") ??
            Path.Combine(Path.GetTempPath(), "new_probes");

        /// <summary>
        /// The repository path of the debugged project.
        /// </summary>
        internal static string RepositoryPath =>
            GetUserEnvironmentVariable("AD_REPOSITORY_PATH") ??
            throw new InvalidOperationException("Environment variable 'AD_REPOSITORY_PATH' undefined.");

        /// <summary>
        /// The git user name (default is null).
        /// </summary>
        internal static string? GitUserName =>
            GetUserEnvironmentVariable("AD_GIT_USER") ??
            null;

        /// <summary>
        /// The git password (default is null).
        /// </summary>
        internal static string? GitPassword =>
            GetUserEnvironmentVariable("AD_GIT_PASSWORD") ??
            null;

        /// <summary>
        /// The full path for beyond compare executable (default is C:\Program Files\Beyond Compare 4\BComp.exe).
        /// </summary>
        internal static string BeyondComparePath =>
            GetUserEnvironmentVariable("AD_BC_PATH") ??
            Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"), "Beyond Compare 4", "BComp.exe");
    }
}
