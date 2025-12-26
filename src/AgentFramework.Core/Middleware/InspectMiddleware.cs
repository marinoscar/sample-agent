using AgentFramework.Core.Agents;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentFramework.Core.Middleware
{
    public static class InspectMiddleware
    {
        public static async Task<AgentRunResponse> RunInspectionAsync(IEnumerable<ChatMessage> messages, AgentThread? thread, AgentRunOptions? options, AIAgent agent, CancellationToken ct)
        {
            PrintDebug(messages, thread, options, agent);
            return await agent.RunAsync(messages, thread, options, ct);
        }

        public static IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingInspectionAsync(IEnumerable<ChatMessage> messages, AgentThread? thread, AgentRunOptions? options, AIAgent agent, CancellationToken ct)
        {
            PrintDebug(messages, thread, options, agent);
            return agent.RunStreamingAsync(messages, thread, options, ct);
        }

        public static async ValueTask<object?> FunctionCallingMiddleware(
                                        AIAgent agent,
                                        FunctionInvocationContext context,
                                        Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next,
                                        CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Function Name: {context!.Function.Name}");
            var result = await next(context, cancellationToken);
            Debug.WriteLine($"Function Call Result: {result}");

            return result;
        }

        private static void PrintDebug(IEnumerable<ChatMessage> messages, AgentThread? thread, AgentRunOptions? options, AIAgent agent)
        {
            var threadFlag = thread != null ? "Available" : "No Available";
            var optionsFlag = options == null ? "Not Available" : JsonSerializer.Serialize(options, new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });

            Debug.WriteLine("---- Agent Inspection Start ----");
            Debug.WriteLine($"Agent: {agent.Name} (ID: {agent.Id})");
            Debug.WriteLine($"Thread: {threadFlag}");
            Debug.WriteLine($"Options: {optionsFlag}");
            Debug.WriteLine($"Message Count: {messages.Count().ToString("N")}");
            Debug.WriteLine("---- Agent Inspection End ----");
        }


    }
}
