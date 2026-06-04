using AutoDebugger.Misc;

namespace AutoDebugger.Probes;

internal class LogSenderFileSystem(string logJsonToAdd) : ILogSender
{
    public async Task<string> SendLog()
    {
        var guid = Guid.NewGuid().ToString();
        Directory.CreateDirectory(Config.NewProbesDirectory);
        await File.WriteAllTextAsync(Path.Combine(Config.NewProbesDirectory, guid + ".json"), logJsonToAdd);
        return guid;
    }
}