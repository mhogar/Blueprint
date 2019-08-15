using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    //TODO: rename this class when I come up with a name for it
    public class Foo
    {
        public InterfaceType TryCast<InterfaceType>()
            where InterfaceType : class
        {
            var interfaceObj = this as InterfaceType;
            if (interfaceObj == null)
            {
                throw new InvalidOperationException(
                    $"{this.GetType().Name} does not extend from or implement {typeof(InterfaceType).Name}");
            }

            return interfaceObj;
        }
    }
}
