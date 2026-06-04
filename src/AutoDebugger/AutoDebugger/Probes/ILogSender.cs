namespace AutoDebugger.Probes;

interface ILogSender
{
    Task<string> SendLog();
}