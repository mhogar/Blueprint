using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Interpreter
{
    public abstract class ContextEvaluatorBase
    {
        public class InvalidIdentifierException : Exception
        {
            public InvalidIdentifierException()
                : base("Identifier not supported in context.")
            {
            }
        }

        public abstract void Evaluate(Interpreter.Identifier identifier, List<string> parameters);
    }
}
