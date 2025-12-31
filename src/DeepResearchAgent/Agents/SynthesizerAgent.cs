using AgentFramework.Core.Agents;
using AgentFramework.Core.Configuration;
using DeepResearchAgent.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Agents
{
    public sealed class SynthesizerAgent : AgentBase
    {
        public SynthesizerAgent(AgentFactory agentFactory) : base(agentFactory)
        {
        }

        protected override AgentConfiguration GetConfiguration()
        {
            return new AgentConfiguration
            {
                Id = "deep-research-synthesizer-agent",
                Name = "Synthesizer Agent",
                Description = "Writes the final report using only supported claims and citations from research findings.",
                Provider = GetProvider(),
                Model = GetModel(),
                Instructions = GetInstructions(),
                ToolMode = "Auto",
                Temperature = null,
                PersistConversation = true,
                ResponseFormat = typeof(SynthesizerAgentResponse),
                ToolList = "datetime"
            };
        }

        protected override string GetInstructions()
        {
            return @"
You are responsible for producing the **final written report**, using only validated research outputs.

## Inputs

* ResearchBrief
* All WebResearchAgent responses
* Current date

## Responsibilities

* Combine findings into a coherent, readable report.
* Preserve factual accuracy and citations.
* Highlight uncertainty honestly.
* Produce a clean, executive-ready structure.

## Required Sections

* Title
* As-of date
* Executive summary (bullets)
* Key findings (with citations)
* Risks & uncertainties
* Recommendations
* Evidence table

## Writing Rules

* Use only provided claims and sources.
* Never introduce new facts.
* If sources disagree, present both views.
* Avoid marketing language.
* Be neutral, precise, and decision-oriented.

## Output Constraints

* Must match the structured response schema.
* No reasoning traces or internal commentary.
* No agent coordination text.


";
        }
    }
}
