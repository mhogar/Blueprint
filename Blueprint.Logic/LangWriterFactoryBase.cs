﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangWriterFactoryBase
    {
        public const byte CLASS_BUILDER = 1;

        protected string _outFile;

        public LangWriterFactoryBase(string outFile)
        {
            _outFile = outFile;
        }

        public abstract byte GetSupportedBuilders();
        public abstract ILangWriter CreateLangWriter();
        public abstract ILangClassBuilder CreateClassBuilder();
    }
}
