using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Agents
{
    public abstract class AgentBase
    {

        protected string GetProvider()
        {
            return "OpenAI";
        }

        protected string GetModel()
        {
            return "gpt-4o";
        }

        protected abstract string GetInstructions();

    }
}
