using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangFileBuilder;
using Blueprint.Logic.LangFactory;
using Blueprint.Logic;

namespace Blueprint.Interpreter.ContextEvaluator
{
    public class BlueprintContextEvaluator : ContextEvaluatorBase, IEvaluateFile
    {
        public BlueprintContextEvaluator() : base("Blueprint")
        {
        }

        public void EvaluateFile(LangFileBuilderBase fileBuilder, Dictionary<string, string> extraParams)
        {
            fileBuilder.WriteFile();
        }
    }
}
