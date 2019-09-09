using Blueprint.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangFileBuilder;
using Blueprint.Logic.LangClassBuilder;
using Blueprint.Interpreter;

namespace Blueprint.Interpreter.ContextEvaluator
{
    public class FileContextEvaluator : ContextEvaluatorBase, IEvaluateVariable, IEvaluateFunction, IEvaluateClass
    {
        private LangFileBuilderBase _fileBuilder;

        public FileContextEvaluator(LangFileBuilderBase fileBuilder) : base("File")
        {
            _fileBuilder = fileBuilder;
        }

        public void EvaluateVariable(VariableObj variableObj, Dictionary<string, string> extraParams)
        {
            TryCastUtil.TryCast<ICreateFileVariable>(_fileBuilder).CreateFileVariable(variableObj);
        }

        public void EvaluateFunction(FunctionObj functionObj, Dictionary<string, string> extraParams)
        {
            TryCastUtil.TryCast<ICreateFileFunction>(_fileBuilder).CreateFileFunction(functionObj);
        }

        public void EvaluateClass(LangClassBuilderBase classBuilder, Dictionary<string, string> extraParams)
        {
            TryCastUtil.TryCast<ICreateFileClass>(_fileBuilder).CreateFileClass(
                classBuilder, GetAccessModifierFromExtraParams(extraParams, AccessModifier.PUBLIC));
        }
    }
}
