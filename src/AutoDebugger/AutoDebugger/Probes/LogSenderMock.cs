namespace AutoDebugger.Probes;

internal class LogSenderMock : ILogSender
{
    public Task<string> SendLog()
    {
        return Task.FromResult(@"{""id"":""67768f88-3643-4776-8a27-703ed7707897"",""type"":""di_log_probe""}");
    }
}