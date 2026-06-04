namespace AutoDebugger.Probes;

interface ILogResult
{
    Task<string?> GetLogResult();
}