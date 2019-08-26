using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangFileBuilder;
using Blueprint.Logic.LangClassBuilder;

namespace Blueprint.Logic.LangFactory
{
    public abstract class LangFactoryBase : TryCastBase
    {
        public LangFactoryBase(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public interface ICreateFileBuilder
    {
        LangFileBuilderBase CreateFileBuilder();
    }

    public interface ICreateClassBuilder
    {
        LangClassBuilderBase CreateClassBuilder();
    }
}
