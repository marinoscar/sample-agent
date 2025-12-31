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
    public sealed class PlannerAgent : AgentBase
    {
        public PlannerAgent(AgentFactory agentFactory) : base(agentFactory)
        {
        }

        protected override AgentConfiguration GetConfiguration()
        {
            return new AgentConfiguration
            {
                Id = "deep_research_planner_agent",
                Name = "Planner Agent",
                Description = "Creates a research plan (sub-questions, suggested queries, and stopping conditions).",
                Provider = GetProvider(),
                Model = GetModel(),
                Instructions = GetInstructions(),
                ToolMode = "Auto",
                Temperature = 0,
                PersistConversation = true,
                ResponseFormat = typeof(PlannerAgentResponse),
                ToolList = "datetime"
            };
        }

        protected override string GetInstructions()
        {
            return @"
# PlannerAgent

You are responsible for converting a clarified research request into an executable research plan.

## Core Responsibilities

* Decompose the topic into logical, non-overlapping sub-questions.
* Ensure coverage of definition, context, evidence, impacts, risks, and implications.
* Define stopping criteria to prevent infinite searching.

## Sub-Question Design Rules

Each sub-question should:

* Be answerable via web research
* Produce factual, citable information
* Avoid overlap with other sub-questions
* Be small enough to search independently

## Planning Output Must Include

* Clear research objective
* List of sub-questions (5–12 typical)
* Suggested search queries per sub-question
* Priority level per sub-question
* Stop conditions

## Stop Conditions Guidelines

* Minimum number of sources per sub-question
* Minimum number of primary sources
* Recency preference (in days)
* Instructions for handling contradictory information

## Constraints

* Do not perform web searches.
* Do not generate final answers.
* Focus only on decomposition and planning quality.

---

";
        }
    }
}
