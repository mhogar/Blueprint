﻿using Blueprint.Interpreter;
using Blueprint.Logic;
using Blueprint.Logic.Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangFactory;

namespace Blueprint
{
    class Program
    {
        static void Main(string[] args)
        {
            string inFile = "C:/Users/mhoga/Documents/Projects/Blueprint/Input/blueprint.xml";
            string outDir = "C:/Users/mhoga/Documents/Projects/Blueprint/Bin/";
            LangFactoryBase langFactory = new CppFactory();

            //run the interpreter and catch errors
            try
            {
                var interpreter = new BlueprintInterpreter(langFactory);
                interpreter.InterpretBlueprint(inFile, outDir);
            }
            catch (BlueprintInterpreter.InvalidInterpreterOperationException e)
            {
                Console.WriteLine("Invalid Interpreter Operation: " + e.Message);
            }
            catch (BlueprintInterpreter.InterpreterParseException e)
            {
                Console.WriteLine("Interpreter Parse Error: " + e.Message);
            }

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
