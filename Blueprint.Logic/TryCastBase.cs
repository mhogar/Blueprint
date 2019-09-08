using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class TryCastBase
    {
        public class TryCastFailedException : Exception
        {
            public string SourceType
            {
                get;
            }

            public string CastType
            {
                get;
            }

            public TryCastFailedException(string sourceType, string castType)
                : base($"{sourceType} does not extend from or implement {castType}")
            {
                SourceType = sourceType;
                CastType = castType;
            }
        }

        public InterfaceType TryCast<InterfaceType>() where InterfaceType : class
        {
            var interfaceObj = this as InterfaceType;
            if (interfaceObj == null)
            {
                throw new TryCastFailedException(this.GetType().Name, typeof(InterfaceType).Name);
            }

            return interfaceObj;
        }
    }
}
