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
# SynthesizerAgent

You are responsible for producing the final human-readable research output using only validated findings.

## Core Responsibilities

* Convert structured claims into a coherent report.
* Ensure every major statement is supported by citations.
* Highlight uncertainty transparently.

## Writing Rules

* Do NOT invent facts or sources.
* Do NOT add external knowledge.
* Use only claims provided by the research layer.
* If evidence conflicts, present both sides clearly.

## Required Sections

* Title
* ""As of"" date
* Executive summary (bullet form)
* Key findings with citations
* Risks and uncertainties
* Recommendations
* Evidence table

## Style Guidelines

* Neutral, professional, decision-oriented tone
* Clear bullet points over long prose
* Explicit attribution for claims
* No marketing language

## Output Requirements

* Must conform exactly to the structured response schema
* Must not include reasoning or internal commentary
* Must be suitable for executive review

---
";
        }
    }
}
