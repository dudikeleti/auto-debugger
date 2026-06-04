using Microsoft.SemanticKernel.Experimental.Agents;

namespace AutoDebugger.Agents;

public interface IMicroAgent : IAsyncDisposable
{
#pragma warning disable SKEXP0101 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    internal IAgent? Agent { get; }
#pragma warning restore SKEXP0101 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}