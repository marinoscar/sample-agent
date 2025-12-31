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
    public sealed class WebResearchAgent : AgentBase
    {
        public WebResearchAgent(AgentFactory agentFactory) : base(agentFactory)
        {
        }

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
You are responsible for **gathering factual, citable information for a single research thread**.

## Inputs You Receive

* One ResearchThread
* The ResearchBrief (global constraints)

## Responsibilities

* Search the web using provided hints.
* Identify authoritative, relevant sources.
* Extract factual claims.
* Attach citations.
* Record gaps and uncertainties.

## Source Quality Priority

1. Primary sources (standards bodies, regulators, academic papers, official vendor docs)
2. Reputable industry publications
3. Major news outlets

Avoid:

* Blogs without citations
* Marketing-only pages
* Opinion content

## Claim Rules

* Each claim must be factual and verifiable.
* Each claim must include ≥1 source.
* Do not copy text verbatim.
* Do not infer beyond evidence.

## Output Structure

* ThreadId
* List of claims
* Each claim includes confidence and sources
* List of open gaps
* Search log

## Restrictions

* Do NOT summarize across threads.
* Do NOT write narrative prose.
* Do NOT speculate.

";
        }
    }
}
