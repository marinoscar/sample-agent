using AgentFramework.Core.Configuration;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public class OpenAIAgent : BaseAgent
    {
        private AIAgent _agent;

        public OpenAIAgent() : this(OpenAISettings.Create("You are a helpful assistant."))
        {

        }

        public OpenAIAgent(OpenAISettings settings)
        {
            
            var tools = new List<AITool>();

            if (settings.EnableWebSearch)
                tools.Add(new WebSearchTool().AsAITool());

            if (settings.EnableCodeInterpreter)
            {
                var codeInterpreter = new CodeInterpreterTool(new CodeInterpreterToolContainer(Guid.NewGuid().ToString()));
                tools.Add(codeInterpreter.AsAITool());
            }

            if (settings.EnableImageGeneration)
                tools.Add(new ImageGenerationTool().AsAITool());

            var client = new OpenAIClient(settings.ApiKey);

            var responsesClient = client.GetResponsesClient(settings.Model);
            _agent = responsesClient.CreateAIAgent(settings.Instructions, tools: tools);

        }




        public async Task StreamAsync(
        string prompt,
        Action<AgentRunResponseUpdate> onResponseUpdate,
        CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                throw new ArgumentException("Prompt cannot be null/empty.", nameof(prompt));

            if (onResponseUpdate == null)
                throw new ArgumentNullException(nameof(onResponseUpdate));


            // If your agent supports cancellation, pass it through (recommended).
            // If it doesn’t, remove cancellationToken from the call below and keep the
            // cancellationToken.ThrowIfCancellationRequested() inside the loop.
            await foreach (var update in _agent.RunStreamingAsync(prompt, cancellationToken: cancellationToken)
                                             .ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Let exceptions in the callback fail the stream (usually what you want).
                onResponseUpdate(update);
            }
        }

        public void Stream(
            string prompt,
            Action<AgentRunResponseUpdate> onResponseUpdate,
            CancellationToken cancellationToken = default)
        {
            StreamAsync(prompt, onResponseUpdate, cancellationToken)
                .GetAwaiter()
                .GetResult();
        }



    }
}
