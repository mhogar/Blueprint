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
            const string OUT_DIR = "C:/Users/mhoga/Documents/Projects/Blueprint/Bin/";
            LangWriterFactoryBase langWriterFactory = new CppWriterFactory(OUT_DIR + "foo");

            var writer = langWriterFactory.CreateLangWriter();
            writer.BeginWriter();
            {
                var classBuilder = langWriterFactory.CreateClassBuilder();
                classBuilder.CreateClass("Foo", AccessModifier.PUBLIC);
                classBuilder.CreateClassProperty(new VariableObj("int", "foo1"), AccessModifier.PROTECTED);
                classBuilder.WriteClass(writer);
            }
            writer.EndWriter();

            Console.ReadLine();
        }
    }
}
