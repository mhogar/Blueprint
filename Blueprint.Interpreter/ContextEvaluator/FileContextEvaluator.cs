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

        public FileContextEvaluator(LangFactoryBase langFactory, LangFileBuilderBase fileBuilder)
            : base(langFactory, "File")
        {
            _fileBuilder = fileBuilder;
        }

        public override uint GetSupportedFlags()
        {
            uint flags = 0;
            flags &= EVALUATE_VARIABLE;
            flags &= EVALUATE_FUNCTION;
            flags &= EVALUATE_CLASS;

            return flags;
        }

        public override void EvaluateVariable(VariableObj variableObj, List<string> extraParams)
        {
            _fileBuilder.CreateFileVariable(variableObj);
        }

        public override void EvaluateProperty(VariableObj variableObj, List<string> extraParams)
        {
            //TODO: return Interpreter.Result instead of throwing exception
            throw new InvalidOperationException("FileContext does not support EvaluateProperty.");
        }

        public override void EvaluateFunction(FunctionObj functionObj, List<string> extraParams)
        {
            _fileBuilder.CreateFileFunction(functionObj);
        }

        public override LangClassBuilderBase EvaluateClass(string className, List<string> extraParams)
        {
            LangClassBuilderBase classBuilder = _langFactory.CreateClassBuilder();
            classBuilder.CreateClass(className);

            _fileBuilder.CreateFileClass(classBuilder);

            return classBuilder;
        }
    }
}
