using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch.Models
{
    [Description("Final report delivered to the user. Produced by WriterAgent.")]
    public class FinalResearchReport
    {
        [Description("Topic copied from the aggregate.")]
        public required string Topic { get; init; }


        [Description("Audience copied from the aggregate.")]
        public string? Audience { get; init; }


        [Description("The complete report content in Markdown.")]
        public required string ResultMarkdown { get; init; }

    }
}
