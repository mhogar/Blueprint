using Blueprint.Interpreter;
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
            string inFile = "C:/Users/mhoga/Documents/Projects/Blueprint/Input/blueprint.xml";
            string outDir = "C:/Users/mhoga/Documents/Projects/Blueprint/Bin/";
            LangFactoryBase langFactory = new CppFactory();

            var interpreter = new BlueprintInterpreter(langFactory);
            interpreter.InterpretBlueprint(inFile, outDir);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
