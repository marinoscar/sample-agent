using AgentFramework.Core.Tools;
using Microsoft.Extensions.AI;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public class AgentToolFactory
    {
        public List<AITool> GetTools(string toolIdsCSV)
        {
            if (string.IsNullOrEmpty(toolIdsCSV)) return default!;
            var toolIds = toolIdsCSV.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return GetTools(toolIds);
        }

        public List<AITool> GetTools(IEnumerable<string> toolIds)
        {
            if (toolIds == null || !toolIds.Any()) return default!;

            var tools = new List<AITool>();
            foreach (var toolId in toolIds)
            {
                var tool = GetToolByName(toolId);
                tools.Add(tool);
            }
            return tools;
        }

        public AITool GetToolByName(string toolId)
        {
            if (string.IsNullOrEmpty(toolId))
                throw new ArgumentNullException(nameof(toolId));

            var toolSearch = toolId.ToLowerInvariant().Trim();
            var codeIntConfig = CodeInterpreterToolContainerConfiguration.CreateAutomaticContainerConfiguration();
            var codeIntContainer = new CodeInterpreterToolContainer(codeIntConfig);

            return toolSearch switch
            {
                "web_search" => new WebSearchTool().AsAITool(),
                "code_interpreter" => new CodeInterpreterTool(codeIntContainer).AsAITool(),
                "image_generation" => new ImageGenerationTool().AsAITool(),
                "datetime" => new DateTimeTool().AsAITool(),
                _ => throw new ArgumentException($"Tool '{toolId}' is not recognized.", nameof(toolId)),
            };
        }
    }
}
