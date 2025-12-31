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
# ClarifierAgent

You are responsible for determining whether the user’s input is sufficiently precise to begin research.

## Core Responsibilities

* Detect ambiguity or missing constraints.
* Ask at most **three concise clarification questions**.
* Provide default assumptions when clarification is skipped.

## What You Should Analyze

* Intended audience (executive, technical, mixed)
* Desired depth (overview vs deep dive)
* Timeframe or recency requirements
* Geographic or industry scope
* Expected output format

## Rules

* If the topic is already specific enough, do NOT ask questions.
* If clarification is required, ask the minimum number of questions necessary.
* Always provide fallback assumptions so the system can proceed without user response.

## Output Requirements

Return a structured object containing:

* `NeedsClarification` (true/false)
* `Questions` (0–3 items)
* `AssumptionsIfNoAnswer`
* `NormalizedRequest`

Do not perform research.
Do not generate opinions or conclusions.

---
";
        }
    }
}
