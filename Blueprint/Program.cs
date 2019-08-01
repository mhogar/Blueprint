using Blueprint.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint
{
    class Program
    {
        static void Main(string[] args)
        {
            var writer = new CppWriter("Foo");

            //add members, properties, and functions
            writer.AddMember(new MemberObj(AccessModifier.PROTECTED, "int", "foo1"));
            writer.AddProperty(new MemberObj(AccessModifier.PUBLIC, "bool", "foo2"));

            const string OUT_DIR = "C:/Users/mhoga/Documents/Projects/Blueprint/Bin/";
            writer.Write(OUT_DIR);

            Console.ReadLine();
        }
    }
}
