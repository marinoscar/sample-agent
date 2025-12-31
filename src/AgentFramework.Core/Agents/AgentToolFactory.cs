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
    /// <summary>
    /// Factory class responsible for creating and managing AI tools for agents.
    /// </summary>
    public class AgentToolFactory
    {
        /// <summary>
        /// Retrieves a collection of AI tools based on a comma-separated string of tool identifiers.
        /// </summary>
        /// <param name="toolIdsCSV">A comma-separated string containing tool identifiers.</param>
        /// <returns>A list of <see cref="AITool"/> instances, or default if the input is null or empty.</returns>
        public List<AITool> GetTools(string toolIdsCSV)
        {
            if (string.IsNullOrEmpty(toolIdsCSV)) return default!;
            var toolIds = toolIdsCSV.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return GetTools(toolIds);
        }

        /// <summary>
        /// Retrieves a collection of AI tools based on an enumerable collection of tool identifiers.
        /// </summary>
        /// <param name="toolIds">An enumerable collection of tool identifiers.</param>
        /// <returns>A list of <see cref="AITool"/> instances, or default if the input is null or empty.</returns>
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

        /// <summary>
        /// Retrieves a specific AI tool by its identifier.
        /// </summary>
        /// <param name="toolId">The unique identifier of the tool. Supported values are:
        /// "web_search", "code_interpreter", "image_generation", "datetime".</param>
        /// <returns>An <see cref="AITool"/> instance corresponding to the specified identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="toolId"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the specified tool identifier is not recognized.</exception>
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
