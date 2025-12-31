using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class ThreadWorkItem
    {
        public string? ThreadId { get; private set; }
        public bool IsDone { get; private set; }

        private ThreadWorkItem() { }

        public static ThreadWorkItem Next(string threadId)
        {
            if (string.IsNullOrWhiteSpace(threadId)) throw new ArgumentException("ThreadId cannot be null/empty.", nameof(threadId));
            return new ThreadWorkItem { ThreadId = threadId, IsDone = false };
        }

        public static ThreadWorkItem Done() => new ThreadWorkItem { ThreadId = null, IsDone = true };
    }
}
