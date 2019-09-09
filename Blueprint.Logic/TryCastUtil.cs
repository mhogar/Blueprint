using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public static class TryCastUtil
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

        public static InterfaceType TryCast<InterfaceType>(object srcObj) where InterfaceType : class
        {
            var interfaceObj = srcObj as InterfaceType;
            if (interfaceObj == null)
            {
                throw new TryCastFailedException(srcObj.GetType().Name, typeof(InterfaceType).Name);
            }

            return interfaceObj;
        }
    }
}
