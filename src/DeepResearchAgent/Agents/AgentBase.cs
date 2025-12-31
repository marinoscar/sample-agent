using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Agents
{
    public abstract class AgentBase
    {

        public string GetProvider()
        {
            return "OpenAI";
        }

        public string GetModel()
        {
            return "gpt-4o";
        }

        public abstract string GetInstructions();

    }
}
