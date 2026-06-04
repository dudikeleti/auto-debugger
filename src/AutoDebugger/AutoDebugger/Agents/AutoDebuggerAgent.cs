using AutoDebugger.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Experimental.Agents;

#pragma warning disable SKEXP0101

namespace AutoDebugger.Agents
{
    public class AutoDebuggerAgent : IMicroAgent
    {
        private const string AgentDescription = "AI assitant for debugging code and fixing code issues.";

        private const string AgentName = "Auto Debugger Agent";
        private readonly List<IAgent> _agents = new();
        private IAgent? _agent;

        public IAgent? Agent => _agent;

        public static AutoDebuggerAgent CreateAgent()
        {
            var agent = new AutoDebuggerAgent();
            agent.Build();
            return agent;
        }

        private void Build()
        {
            _agent = AgentHelper.CreateAgent(
                AgentName,
                Templates.Templates.AutoDebuggerExpert,
                AgentDescription,
                GetAgents().Select(a => a.AsPlugin()).ToArray());

            _agents.Add(_agent);
        }

        private IEnumerable<IAgent> GetAgents()
        {
            yield return
                GetMicroAgent<SymbolRetrieverPlugin>(
                    "Symbol Retriever Agent",
                    "Provide symbol information for type or type member using only the provided tools.",
                    "An agent that provides symbols information for given c# type or type member (e.g. class, method, and property)");

            yield return
                GetMicroAgent<SourceCodeRetrieverPlugin>(
                    "Source Code Retriever Agent",
                    "Provide source code for given file path and line range.",
                    "An agent that provide source code for given path (optionally provide source code for range of lines)");

            yield return
                GetMicroAgent<FindUsagesPlugin>(
                    "Find Code Usages Agent",
                    "Provide usages information for specific method.",
                    "An agent that provides code usages for specific method.");

            yield return
                GetMicroAgent<SendLogPlugin>(
                    "Send Log Agent",
                    "Add log",
                    "An agent that can add logs.");

            yield return
                GetMicroAgent<SendHttpGetRequestPlugin>(
                    "Send HTTP Get Request Agent",
                    "Send HTTP GET request to specific URL.",
                    "An agent that can send HTTP GET request.");

            yield return
                GetMicroAgent<GetLogResultPlugin>(
                    "Get Log Result Agent",
                    "Get log snapshot result",
                    "An agent that can get log snapshot result.");

            yield return
            GetMicroAgent<CodeComparerPlugin>(
                "Code Comparison Agent",
                "Compare two pieces of code",
                "An agent that can compare two pieces of code.");

            yield return
                GetMicroAgent<GitCommiterPlugin>(
                    "Git Commit Agent",
                    "Commit code changes",
                    "An agent that can commit code changes.");

            /*
            /*yield return
                GetMicroAgentFromYamlFile(Helper.GetPathForYamlFile("User.yaml"));

            yield return
                GetMicroAgentFromTemplateConfig(
                    "Code Issue Solver",
                    Templates.Templates.AutoDebuggerExpert,
                    "An agent that can understand code issues and use various methodology to debug the code and suggest solution.");
            yield return
                GetMicroPromptAgent(Helper.GetPathForYamlFile("ExpertCodeDebugger.yaml"));
            */
        }

        private IAgent GetMicroAgentFromTemplateConfig(
            string name,
            string template,
            string description)
        {
            var config = new PromptTemplateConfig(template)
            {
                Name = name,
                Description = description
            };

            var microAgent = AgentHelper.CreateAgent(config);
            _agents.Add(microAgent);
            return microAgent;
        }

        private IAgent GetMicroAgentFromYamlFile(string templatePath)
        {
            var microAgent = AgentHelper.CreateAgent(templatePath);
            _agents.Add(microAgent);
            return microAgent;
        }

        private IAgent GetMicroAgent<T>(
            string agentName,
            string agentInstructions,
            string agentDescription)
            where T : DiagnosticPlugin, new()
        {
            var microAgent = AgentHelper.CreateAgent(
                agentName,
                agentInstructions,
                agentDescription,
                AgentHelper.CreateAgentPlugin<T>());

            _agents.Add(microAgent);
            return microAgent;
        }

        public async Task DeleteAgentsAsync()
        {
            foreach (var agent in _agents)
            {
                try
                {
                    await agent.DeleteAsync();
                }
                catch
                {
                    Console.WriteLine($"Error: Fail to delete agent {agent.Id} ({agent.Name})");
                }
            }

            _agents.Clear();
        }

        public async ValueTask DisposeAsync()
        {
            await DeleteAgentsAsync();
        }
    }
}
