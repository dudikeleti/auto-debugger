using AutoDebugger.Agents;

namespace Example
{
    internal class Program
    {
        private const string GoalExample =
            "I'm trying to unbook room course without success. I'm using this URI: http://127.0.0.1:6001/Rooms/PP-201/unbook?course=csharp " +
            "The bug might be related to UnbookRoom method in RoomsController class, but the root issue might be in the calling or called methods. " +
            "Help me find the issue and suggest a fix based on the instructions that you have.";

        static async Task Main(string[] args)
        {
            var agent = AutoDebuggerAgent.CreateAgent();
            await using var runner = new AgentRunner(agent, GoalExample);
            await runner.InvokeAsync();
        }
    }
}
