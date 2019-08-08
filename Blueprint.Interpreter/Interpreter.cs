using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Interpreter
{
    public class Interpreter
    {
        private Stack<ContextEvaluatorBase> _contextStack;

        public enum Identifier
        {
            VARIABLE,
            FUNCTION,
            CLASS,
            PROPERTY
        }

        public Interpreter()
        {
            _contextStack = new Stack<ContextEvaluatorBase>();
        }
    }
}
