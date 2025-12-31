using DeepResearchAgent.DTO;
using DeepResearchAgent.Executors;
using Microsoft.Agents.AI.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent
{
    public class DeepResearchWorkflow
    {
        private Func<ClarifierAgentResponse?, bool> NeedsClarification(bool expected) =>
            r => r is not null && r.NeedsClarification == expected;

        private Func<ThreadWorkItem?, bool> HasMoreThreads(bool expected) =>
            w => w is not null && w.IsDone == !expected; // expected=true => IsDone=false

        public Workflow Build(
            IntakeExecutor intake,
            ScopeExecutor scope,
            ClarificationOutputExecutor clarificationOutput,
            InitializeResearchQueueExecutor initQueue,
            WebResearchExecutor webResearch,
            AggregateAndNextExecutor aggregateAndNext,
            SynthesisExecutor synthesis,
            FinalizeExecutor finalize)
        {
            if (intake is null) throw new ArgumentNullException(nameof(intake));
            if (scope is null) throw new ArgumentNullException(nameof(scope));
            if (clarificationOutput is null) throw new ArgumentNullException(nameof(clarificationOutput));
            if (initQueue is null) throw new ArgumentNullException(nameof(initQueue));
            if (webResearch is null) throw new ArgumentNullException(nameof(webResearch));
            if (aggregateAndNext is null) throw new ArgumentNullException(nameof(aggregateAndNext));
            if (synthesis is null) throw new ArgumentNullException(nameof(synthesis));
            if (finalize is null) throw new ArgumentNullException(nameof(finalize));

            // Graph:
            // Intake -> Scope
            // Scope -> ClarificationOutput (if NeedsClarification)
            // Scope -> InitQueue (if !NeedsClarification)
            // InitQueue -> WebResearch (if HasMoreThreads)
            // WebResearch -> AggregateAndNext
            // AggregateAndNext -> WebResearch (if HasMoreThreads)
            // AggregateAndNext -> Synthesis (if !HasMoreThreads)
            // Synthesis -> Finalize
            var workflow = new WorkflowBuilder(intake)
                .AddEdge(intake, scope)
                .AddEdge(scope, clarificationOutput, NeedsClarification(expected: true))
                .AddEdge(scope, initQueue, NeedsClarification(expected: false))
                .AddEdge(initQueue, webResearch, HasMoreThreads(expected: true))
                .AddEdge(webResearch, aggregateAndNext)
                .AddEdge(aggregateAndNext, webResearch, HasMoreThreads(expected: true))
                .AddEdge(aggregateAndNext, synthesis, HasMoreThreads(expected: false))
                .AddEdge(synthesis, finalize)
                .Build();

            return workflow;
        }
    }
}
