using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class SearchLogEntry
    {
        /// <summary>
        /// The search query used.
        /// </summary>
        public required string Query { get; init; }

        /// <summary>
        /// Notes such as "too broad", "good sources found", "mostly low-quality blogs", etc.
        /// </summary>
        public string? Notes { get; init; }
    }
}
