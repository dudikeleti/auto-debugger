using AutoDebugger.Misc;

namespace AutoDebugger.Probes;

internal class LogResultFileSystem : ILogResult, IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly string _logId;
    private string? _result;

    public LogResultFileSystem(string idToSearch)
    {
        _semaphoreSlim = new SemaphoreSlim(0);
        _logId = idToSearch;
        _watcher = new FileSystemWatcher(Config.SnapshotDirectory, idToSearch + ".json")
        {
            IncludeSubdirectories = true,
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.FileName
        };

        _watcher.Created += _watcher_Created;
    }

    private void _watcher_Created(object sender, FileSystemEventArgs e)
    {
        const int maxRetries = 5;
        const int sleepForMs = 500;

        try
        {
            for (int i = 1; i <= maxRetries; ++i)
            {
                try
                {
                    using var fileStream = File.OpenRead(e.FullPath);
                    using var stream = new StreamReader(fileStream);
                    _result = stream.ReadToEnd();
                    break;
                }
                catch (IOException) when (i < maxRetries)
                {
                    Thread.Sleep(sleepForMs);
                }
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<string?> GetLogResult()
    {
        await _semaphoreSlim.WaitAsync(TimeSpan.FromMinutes(1));
        if (!string.IsNullOrEmpty(Volatile.Read(ref _result)))
        {
            return _result;
        }

        // too late, search file system if it already exist
        var files = Directory.GetFiles(Config.SnapshotDirectory, _logId + ".json", SearchOption.AllDirectories);
        if (files.Length == 0)
        {
            return "Log result not found";
        }

        _result = await File.ReadAllTextAsync(files.First());
        return _result;
    }

    public void Dispose()
    {
        _watcher.Dispose();
        _semaphoreSlim.Dispose();
    }
}