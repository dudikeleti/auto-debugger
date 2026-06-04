namespace AutoDebugger.Probes;

internal class LogResultHttp : ILogResult
{
    public Task<string?> GetLogResult()
    {
        return Task.FromResult<string?>("Not implemented");
    }
}