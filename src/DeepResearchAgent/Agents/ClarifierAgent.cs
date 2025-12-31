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
    public class ClarifierAgent : AgentBase
    {
        public ClarifierAgent(AgentFactory agentFactory) : base(agentFactory)
        {
        }

        protected override AgentConfiguration GetConfiguration()
        {
            return new AgentConfiguration
            {
                Id = "deep_research_clarifier_agent",
                Name = "Clarifier Agent",
                Description = "Determines whether clarifying questions are necessary and produces assumptions if not.",
                Provider = GetProvider(),
                Model = GetModel(),
                Instructions = GetInstructions(),
                ToolMode = "Auto",
                Temperature = 0,
                PersistConversation = true,
                ResponseFormat = typeof(ClarifierAgentResponse),
                ToolList = "datetime"
            };
        }

        protected override string GetInstructions()
        {
            return @"
You are responsible for **scoping the research correctly before any research begins**.

## Purpose

Convert a vague user request into a precise, executable ResearchBrief.

## Responsibilities

* Detect ambiguity or missing constraints.
* Ask up to **three** clarifying questions when necessary.
* Provide default assumptions so execution can proceed without user input.
* Produce a ResearchBrief when clarification is sufficient.

## What to Analyze

* Intended audience
* Desired depth and format
* Timeframe / recency
* Scope boundaries (industry, geography)

## Rules

* Ask the fewest questions possible.
* If a reasonable default exists, prefer assumptions over questions.
* Never perform research.
* Never invent facts.

## Output Requirements

Return one of:

### A) Needs clarification

* NeedsClarification = true
* 1–3 concise questions
* Assumptions if unanswered

### B) Ready to proceed

* NeedsClarification = false
* Fully populated ResearchBrief


";
        }
    }
}
