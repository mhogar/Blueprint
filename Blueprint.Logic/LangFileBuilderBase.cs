﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangFileBuilderBase : ISupportedFlags
    {
        public static readonly UInt32 FILE_VARAIBLE = 0x1;
        public static readonly UInt32 FILE_FUNCTION = 0x2;
        public static readonly UInt32 FILE_CLASS = 0x4;

        public abstract UInt32 GetSupportedFlags();
        public abstract void CreateFileVariable(VariableObj variableObj);
        public abstract void CreateFileFunction(FunctionObj functionObj);
        public abstract void CreateFileClass(LangClassBuilderBase classBuilder);
        public abstract void WriteFile(ILangWriter langWriter);
    }
}
