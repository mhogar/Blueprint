using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangFileBuilderBase : ISupportedFlags
    {
        public static readonly uint FILE_VARAIBLE = 0x1;
        public static readonly uint FILE_FUNCTION = 0x2;
        public static readonly uint FILE_CLASS = 0x4;

        public LangFileBuilderBase(string filename)
        {
            Filename = filename;
        }

        public string Filename
        {
            get;
        }

        public abstract uint GetSupportedFlags();
        public abstract void CreateFileVariable(VariableObj variableObj);
        public abstract void CreateFileFunction(FunctionObj functionObj);
        public abstract void CreateFileClass(
            LangClassBuilderBase classBuilder, AccessModifier accessModifier = AccessModifier.PUBLIC);
        public abstract void WriteFile(LangWriterBase langWriter);
    }
}
