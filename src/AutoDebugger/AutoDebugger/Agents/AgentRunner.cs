using System.Diagnostics;
using AutoDebugger.Misc;
using AutoDebugger.Plugins;

#pragma warning disable SKEXP0101

namespace AutoDebugger.Agents
{
    public class AgentRunner(IMicroAgent agent, string initialObjective) : IAsyncDisposable
    {
        private AgentTrace? _tracer;

        public async Task InvokeAsync()
        {
            if (agent.Agent == null)
            {
                throw new ArgumentNullException("agent");
            }

            Prepare();

            try
            {
                Console.WriteLine();

                var timer = Stopwatch.StartNew();

                await InvokeWithDiagnostics();

                var duration = timer.Elapsed;

                _tracer?.WriteTrace($"Duration: {duration}");

                foreach (var diagnostic in DiagnosticPlugin.Counts.OrderBy(kvp => kvp.Key))
                {
                    _tracer?.WriteTrace($"{diagnostic.Key} #{diagnostic.Value}");
                }

                _tracer?.WriteTrace("=====================================");

                foreach (var diagnostic in DiagnosticPlugin.Traces)
                {
                    _tracer?.WriteTrace($"# {diagnostic.Function}");

                    foreach (var parameter in diagnostic.Parameters)
                    {
                        _tracer?.WriteTrace($"  - {parameter.Key}:{parameter.Value}");
                    }

                    _tracer?.WriteTrace($"  > {diagnostic.Value}");
                }
            }
            catch (Exception exception)
            {
                _tracer?.WriteError(exception.ToString());
            }
        }

        private async Task InvokeWithDiagnostics()
        {
            _tracer?.WriteInput($"Agent: {agent.Agent!.Name}");
            _tracer?.WriteInput($"Model: {agent.Agent!.Model}");
            _tracer?.WriteInput($"User initial objective: {initialObjective}");

            var thread = await agent.Agent!.NewThreadAsync();
            try
            {
                _tracer?.WriteLine();
                _tracer?.WriteTrace("Start =====================================");
                _tracer?.WriteTrace($"[{thread.Id}]");

                string? userMessage = initialObjective;
                while (!string.IsNullOrWhiteSpace(userMessage) && userMessage != "exit")
                {
                    // _tracer?.WriteInput($"User: {userMessage}");

                    await foreach (var responseMessage in thread.InvokeAsync(agent.Agent, userMessage))
                    {
                        _tracer?.WriteLine();
                        _tracer?.WriteTrace($"[{responseMessage.Id}]");
                        _tracer?.WriteAssistant($"{responseMessage.Content}");
                    }

                    userMessage = Console.ReadLine();
                }
            }
            finally
            {
                _tracer?.WriteTrace("Finish =====================================");
                await thread.DeleteAsync();
            }
        }

        private void Prepare()
        {
            DiagnosticPlugin.Reset();

            Directory.CreateDirectory(Config.ResultDirectory);
            Directory.CreateDirectory(Config.SnapshotDirectory);
            Directory.CreateDirectory(Config.NewProbesDirectory);

            _tracer = new AgentTrace(GetDiagnosticFile());
        }

        private StreamWriter? GetDiagnosticFile()
        {
            try
            {
                return File.CreateText(Path.Combine(Config.ResultDirectory, $"{agent.Agent!.Name}-{agent.Agent.Model}-{DateTime.Now:yyMMdd-HHmmss}.txt"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_tracer != null)
            {
                await _tracer.DisposeAsync();
            }

            await agent.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
