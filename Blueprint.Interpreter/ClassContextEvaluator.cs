using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic;

namespace Blueprint.Interpreter
{
    public class ClassContextEvaluator : ContextEvaluatorBase
    {
        private LangClassBuilderBase _classBuilder;

        public ClassContextEvaluator(LangClassBuilderBase classBuilder)
        {
            _classBuilder = classBuilder;
        }

        public override void Evaluate(Interpreter.Identifier identifier, List<string> parameters)
        {
            switch(identifier)
            {
                case Interpreter.Identifier.FUNCTION:

                    break;
                case Interpreter.Identifier.VARIABLE:
                    break;
                case Interpreter.Identifier.PROPERTY:
                    break;
                case Interpreter.Identifier.CLASS:
                    break;
                default:
                    throw new InvalidIdentifierException();
            }
        }
    }
}
