using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public interface ILangCoreWriter
    {
        void CreateVariable(VariableObj variableObj);
        void CreateFunction(FunctionObj functionObj);
        void Write();
    }
}
