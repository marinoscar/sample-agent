using AgentFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch
{
    public class AgentBuilder
    {

        private const string DefaultModel = "gpt-4o";
        private const bool PersistConversation = true;
        private const string DefaultTools = "web_search,datetime";
        private const string DefaultProvider = "OpenAI";

        public AgentConfiguration CreateScopingAgentConfig()
        {
            var instructions = @"
Role: Convert the user’s prompt into a complete, research-ready scope or ask targeted clarifying questions.

Instructions to the LLM

You are the ScopingAgent. Your job is to transform the user’s input into a ResearchTopic object.

Determine whether the user provided enough detail to proceed.

If not enough: set NeedsClarification = true and generate specific, minimal clarification questions that unblock planning.

If enough: set NeedsClarification = false and fill the scope fields as best as possible.

Keep questions crisp. Prefer 5 or fewer questions unless absolutely necessary.

Infer defaults when reasonable:

Audience: ""exec"" | ""technical"" | ""mixed"" (default ""mixed"" if unclear)

OutputFormat: ""brief"" | ""deep_dive"" | ""comparison"" | ""bullets"" | ""slides_outline"" (default ""deep_dive"" if user asked for “deep research”)

Constraints: include timeframe, geography, allowed sources, domain constraints, “must include citations,” etc.

Populate DetailedResearchScope as a short paragraph describing what “done” looks like.

Output only valid JSON matching the ResearchTopic schema. No prose.
";
            var config = new AgentConfiguration
            {
                Id = "dr-scoping-agent",
                Name = "Scoping Agent",
                Description = "Defines and refines the research scope based on user input.",
                Provider = DefaultProvider,
                Model = DefaultModel,
                ToolMode = "Auto",
                Instructions = instructions,
                Temperature = 0f,
                PersistConversation = PersistConversation,
                ResponseFormat = null,
                ToolList = DefaultTools
            };

            return config;
        }

        public AgentConfiguration CreatePlanningAgentConfig()
        {
            var instructions = @"";
            var config = new AgentConfiguration
            {
                Id = "dr-planning-agent",
                Name = "Planning Agent",
                Provider = DefaultProvider,
                Model = DefaultModel,
                ToolMode = "Auto",
                Instructions = instructions,
                Temperature = 0f,
                PersistConversation = PersistConversation,
                ResponseFormat = null,
                ToolList = DefaultTools
            };

            return config;
        }

        public AgentConfiguration CreateResearchAgentConfig()
        {
            var instructions = @"";
            var config = new AgentConfiguration
            {
                Id = "dr-research-agent",
                Name = "Research Agent",
                Provider = DefaultProvider,
                Model = DefaultModel,
                ToolMode = "Auto",
                Instructions = instructions,
                Temperature = 0f,
                PersistConversation = PersistConversation,
                ResponseFormat = null,
                ToolList = DefaultTools
            };

            return config;
        }

        public AgentConfiguration CreateWriterAgentConfig()
        {
            var instructions = @"";
            var config = new AgentConfiguration
            {
                Id = "dr-research-agent",
                Name = "Research Agent",
                Provider = DefaultProvider,
                Model = DefaultModel,
                ToolMode = "Auto",
                Instructions = instructions,
                Temperature = 0f,
                PersistConversation = PersistConversation,
                ResponseFormat = null,
                ToolList = DefaultTools
            };

            return config;
        }


    }
}
