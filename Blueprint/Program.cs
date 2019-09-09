using Blueprint.Interpreter;
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
    public class Program
    {
        public struct ParseArgsResult
        {
            public string inFile;
            public string outDir;
            public LangFactoryBase langFactory;
        }

        public class ParseArgsException : Exception
        {
            public enum ParseErrorCode
            {
                INVALID_ARG_COUNT,
                INVALID_FLAG,
                INVALID_LANG_STRING,
                DUPLICATE_FLAG,
                REQUIRED_FLAG_MISSING
            }

            public ParseErrorCode ErrorCode
            {
                get;
            }

            public ParseArgsException(ParseErrorCode errorCode, string message) : base(message)
            {
                ErrorCode = errorCode;
            }
        }

        public static ParseArgsResult ParseArgs(string[] args)
        {
            string inFile = "";
            string outDir = "";
            LangFactoryBase langFactory = null;

            //check for invalid number of arguments
            if (args.Length % 2 != 0)
            {
                throw new ParseArgsException(ParseArgsException.ParseErrorCode.INVALID_ARG_COUNT, "Invalid number of arguments");
            }

            //parse the args
            for (uint i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                switch (arg)
                {
                    //TODO: add help flags (-h, --help) to print the usage and arg options
                    //inFile flags
                    case "-i":
                    case "--input":
                        if (inFile != "")
                        {
                            throw new ParseArgsException(ParseArgsException.ParseErrorCode.DUPLICATE_FLAG, $"Duplicate flag: ${arg}, input filename already set");
                        }

                        inFile = args[++i];
                        break;
                    //outDir flags
                    case "-o":
                    case "--outdir":
                        if (outDir != "")
                        {
                            throw new ParseArgsException(ParseArgsException.ParseErrorCode.DUPLICATE_FLAG, $"Duplicate flag: ${arg}, output directory already set");
                        }

                        outDir = args[++i];
                        break;
                    //lang flags
                    case "-l":
                    case "--lang":
                        if (langFactory != null)
                        {
                            throw new ParseArgsException(ParseArgsException.ParseErrorCode.DUPLICATE_FLAG, $"Duplicate flag: ${arg}, target lang already set");
                        }

                        //evaluate the lang string and create the factory
                        string langStr = args[++i];
                        switch (langStr)
                        {
                            case "Cpp":
                                langFactory = new CppFactory();
                                break;
                            default:
                                throw new ParseArgsException(ParseArgsException.ParseErrorCode.INVALID_LANG_STRING, "Invalid lang string: " + langStr);
                        }
                        break;
                    default:
                        throw new ParseArgsException(ParseArgsException.ParseErrorCode.INVALID_FLAG, "Invalid flag: " + arg);
                }
            }

            //check all required args were set
            {
                if (inFile == "")
                {
                    throw new ParseArgsException(ParseArgsException.ParseErrorCode.REQUIRED_FLAG_MISSING, "Input filename not set");
                }

                if (outDir == "")
                {
                    throw new ParseArgsException(ParseArgsException.ParseErrorCode.REQUIRED_FLAG_MISSING, "Output directory not set");
                }

                if (langFactory == null)
                {
                    throw new ParseArgsException(ParseArgsException.ParseErrorCode.REQUIRED_FLAG_MISSING, "Target lang not set");
                }
            }

            //create the result struct and return
            ParseArgsResult parseArgsResult;
            parseArgsResult.inFile = inFile;
            parseArgsResult.outDir = outDir;
            parseArgsResult.langFactory = langFactory;

            return parseArgsResult;
        }

        public static void Main(string[] args)
        {
            //parse the args and catch errors
            try
            {
                ParseArgsResult parseArgsResult = ParseArgs(args);

                //run the interpreter and catch errors
                try
                {
                    var interpreter = new BlueprintInterpreter(parseArgsResult.langFactory);
                    interpreter.InterpretBlueprint(parseArgsResult.inFile, parseArgsResult.outDir);
                }
                catch (BlueprintInterpreter.InterpreterParseException e)
                {
                    Console.WriteLine("Interpreter Parse Error: " + e.Message);
                }
                catch (BlueprintInterpreter.BlueprintSchemaValidationException e)
                {
                    Console.WriteLine("Blueprint Schema Validation Error: " + e.Message);
                }

                Console.WriteLine("Completed Successfully. Output files in " + parseArgsResult.outDir);
            }
            catch (ParseArgsException e)
            {
                Console.WriteLine("Command Line Argument Error: " + e.Message);
            }

            //wait for user to exit program
            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
