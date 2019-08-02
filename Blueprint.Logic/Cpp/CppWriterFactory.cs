﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class CppWriterFactory : LangWriterFactoryBase
    {
        public CppWriterFactory(string outDir) : base(outDir)
        {
        }

        public override byte GetSupportedWriters()
        {
            byte flags = 0;
            flags &= CLASS_WRITER;

            return flags;
        }

        public override ILangWriter CreateLangWriter()
        {
            return new CppWriter(_outFile);
        }

        public override ILangClassBuilder CreateClassBuilder()
        {
            return new CppClassBuilder();
        }
    }
}
