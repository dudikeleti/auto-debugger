using Microsoft.SemanticKernel.Experimental.Agents;
using Microsoft.SemanticKernel;
using AutoDebugger.Misc;
using AutoDebugger.Plugins;

#pragma warning disable SKEXP0101

namespace AutoDebugger.Agents
{
    internal class AgentHelper
    {
        internal static KernelPlugin CreateAgentPlugin<T>() where T : DiagnosticPlugin, new()
        {
            var tool = new T();
            return KernelPluginFactory.CreateFromObject(tool);
        }

        internal static IAgent CreateAgent(
            string templatePath,
            params KernelPlugin[] plugins)
        {
            return new AgentBuilder()
                .WithOpenAIChatCompletion(Config.ModelName, Config.ApiKey)
                .FromTemplatePath(templatePath)
                .WithPlugins(plugins)
                .BuildAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        internal static IAgent CreateAgent(
            PromptTemplateConfig config,
            params KernelPlugin[] plugins)
        {
            return new AgentBuilder()
                .WithOpenAIChatCompletion(Config.ModelName, Config.ApiKey)
                .WithName(config.Name)
                .WithDescription(config.Description)
                .WithInstructions(config.Template)
                .WithPlugins(plugins)
                .BuildAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        internal static IAgent CreateAgent(
            string agentName,
            string agentInstructions,
            string agentDescription,
            params KernelPlugin[] plugins)
        {
            return new AgentBuilder()
                .WithOpenAIChatCompletion(Config.ModelName, Config.ApiKey)
                .WithName(agentName)
                .WithInstructions(agentInstructions)
                .WithDescription(agentDescription)
                .WithPlugins(plugins)
                .BuildAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}
