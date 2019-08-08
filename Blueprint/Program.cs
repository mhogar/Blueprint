using Blueprint.Logic;
using Blueprint.Logic.Cpp;
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
            LangFactoryBase langWriterFactory = new CppFactory();

            var writer = langWriterFactory.CreateLangWriter(OUT_DIR + "Foo");
            writer.BeginWriter();
            {
                var fileBuilder = langWriterFactory.CreateFileBuilder();
                {
                    var classBuilder = langWriterFactory.CreateClassBuilder();
                    classBuilder.CreateClass("Foo", AccessModifier.PUBLIC);
                    classBuilder.CreateClassProperty(new VariableObj("int", "foo1"), AccessModifier.PROTECTED);

                    fileBuilder.CreateFileClass(classBuilder);
                }

                fileBuilder.WriteFile(writer);
            }
            writer.EndWriter();

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
