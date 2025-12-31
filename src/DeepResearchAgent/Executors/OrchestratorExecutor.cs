using DeepResearchAgent.Agents;
using DeepResearchAgent.DTO;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeepResearchAgent.Executors
{
    public class OrchestratorExecutor : Executor<ChatMessage, OrchestratorAgentResponse>
    {
        private readonly OrchestratorAgent _agentFactory;
        private readonly AIAgent _agent;
        private readonly AgentThread _agentThread;
        public OrchestratorExecutor(OrchestratorAgent agent) : base(nameof(OrchestratorExecutor))
        {
            _agentFactory = agent ?? throw new ArgumentNullException(nameof(agent));
            _agent = _agentFactory.CreateAgent();
            _agentThread = _agent.GetNewThread();
        }

        public override async ValueTask<OrchestratorAgentResponse> HandleAsync(ChatMessage message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var input = message.Text;
            var agent = _agentFactory.CreateAgent();
            var response = await agent.RunAsync(input, _agentThread, options: new AgentRunOptions()
            {

            }, cancellationToken);
            
            var output = JsonSerializer.Deserialize<OrchestratorAgentResponse>(response.Text, new JsonSerializerOptions(){ 
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            await context.QueueStateUpdateAsync("orchestrator_status", response.Text, cancellationToken);

            return output!;
        }
    }
}
