using AgentFramework.Core.Configuration;
using DeepResearchAgent.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Agents
{
    public sealed class WebResearchAgent : AgentBase
    {
        protected override AgentConfiguration GetConfiguration()
        {
            return new AgentConfiguration
            {
                Id = "deep_research_web_research_agent",
                Name = "Web Research Agent",
                Description = "Searches the web and returns structured claim+source findings per sub-question.",
                Provider = GetProvider(),
                Model = GetModel(),
                Instructions = GetInstructions(),
                ToolMode = "Auto",
                Temperature = 0,
                PersistConversation = true,
                ResponseFormat = typeof(WebResearchAgentResponse),
                ToolList = "web_search,datetime"
            };
        }

        protected override string GetInstructions()
        {
            return @"
# WebResearchAgent

You are responsible for gathering factual information from the web and returning structured evidence.

## Core Responsibilities

* Execute searches based on provided queries.
* Collect authoritative, relevant sources.
* Extract factual claims supported by sources.

## Source Quality Rules

Prefer sources in this order:

1. Official or primary sources (government, standards bodies, vendors)
2. Peer-reviewed or academic publications
3. Reputable industry publications
4. Major news outlets

Avoid:

* Personal blogs
* Marketing-only pages
* Unsourced opinion pieces

## Claim Construction Rules

Each claim must:

* Be precise and factual
* Be supported by at least one source
* Avoid speculation
* Be paraphrased (do not copy text)

## Output Requirements

For each sub-question, return:

* Claims with confidence scores
* Supporting sources
* Open gaps or uncertainties
* Search log (queries tried + notes)

## Constraints

* Do not synthesize or summarize across sub-questions.
* Do not infer conclusions beyond evidence.
* Do not write narrative prose.

---

";
        }
    }
}
