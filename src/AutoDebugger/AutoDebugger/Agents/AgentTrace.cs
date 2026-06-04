namespace AutoDebugger.Agents;

internal class AgentTrace(StreamWriter? writer) : IAsyncDisposable
{
    internal const ConsoleColor ColorError = ConsoleColor.DarkRed;
    internal const ConsoleColor ColorTrace = ConsoleColor.DarkGray;
    internal const ConsoleColor ColorInput = ConsoleColor.Green;
    internal const ConsoleColor ColorAssistant = ConsoleColor.Cyan;

    internal void WriteInput(string input)
    {
        WriteLine($"{input}", ColorInput);
    }

    internal void WriteTrace(string trace)
    {
        WriteLine($"{trace}", ColorTrace);
    }

    internal void WriteAssistant(string assistant)
    {
        WriteLine($"# {assistant}", ColorAssistant);
    }

    internal void WriteError(string trace)
    {
        WriteLine($"{trace}", ColorError);
    }

    private void WriteLine(string message, ConsoleColor color)
    {
        Write(message, color);
        WriteLine();
    }

    private void Write(string message, ConsoleColor color)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        writer?.Write(message);
        Console.Write(message);

        Console.ForegroundColor = currentColor;
    }

    internal void WriteLine()
    {
        writer?.WriteLine();
        Console.WriteLine();
    }

    public async ValueTask DisposeAsync()
    {
        if (writer != null)
        {
            await writer.DisposeAsync();
        }
    }
}