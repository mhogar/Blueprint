using Blueprint.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Interpreter
{
    public class FileContextEvaluator : ContextEvaluatorBase
    {
        private LangFileBuilderBase _fileBuilder;

        public FileContextEvaluator(LangFileBuilderBase fileBuilder)
            : base("File")
        {
            _fileBuilder = fileBuilder;
        }

        public override uint GetSupportedFlags()
        {
            uint flags = 0;
            flags |= EVALUATE_VARIABLE;
            flags |= EVALUATE_FUNCTION;
            flags |= EVALUATE_CLASS;

            return flags;
        }

        public override void EvaluateVariable(VariableObj variableObj, Dictionary<string, string> extraParams)
        {
            _fileBuilder.CreateFileVariable(variableObj);
        }

        public override void EvaluateProperty(VariableObj variableObj, Dictionary<string, string> extraParams)
        {
            throw new InvalidOperationException("FileContext does not support EvaluateProperty.");
        }

        public override void EvaluateFunction(FunctionObj functionObj, Dictionary<string, string> extraParams)
        {
            _fileBuilder.CreateFileFunction(functionObj);
        }

        public override void EvaluateConstructor(List<VariableObj> constructorParams, Dictionary<string, string> extraParams)
        {
            throw new InvalidOperationException("FileContext does not support EvaluateConstructor.");
        }

        public override void EvaluateClass(LangClassBuilderBase classBuilder, Dictionary<string, string> extraParams)
        {
            _fileBuilder.CreateFileClass(classBuilder);
        }
    }
}
