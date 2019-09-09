using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blueprint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Tests
{
    [TestClass]
    public class ParseArgsTests
    {
        private string[] _args;
        private const int INPUT_FILE_FLAG_INDEX = 0;
        private const int OUTPUT_DIR_FLAG_INDEX = 2;
        private const int LANG_FLAG_INDEX = 4;

        private void ValidateParseArgs()
        {
            Program.ParseArgsResult result = Program.ParseArgs(_args);
            Assert.AreEqual(_args[INPUT_FILE_FLAG_INDEX + 1], result.inFile);
            Assert.AreEqual(_args[OUTPUT_DIR_FLAG_INDEX + 1], result.outDir);
            Assert.AreEqual(_args[LANG_FLAG_INDEX + 1], result.langFactory.Name);
        }

        private void TestParseArgsFails(Program.ParseArgsException.ParseErrorCode expectedErrorCode)
        {
            Program.ParseArgsException e = Assert.ThrowsException<Program.ParseArgsException>(() => ValidateParseArgs());
            Assert.AreEqual(expectedErrorCode, e.ErrorCode);
        }

        private void TestInputFlag(int index, string[] validFlags)
        {
            //test all valid flags flags
            foreach (string validFlag in validFlags)
            {
                _args[index] = validFlag;
                ValidateParseArgs();
            }

            //test duplicate invalid
            List<string> argsList = _args.ToList();
            argsList.AddRange(new string[] { validFlags[0], "foo" });
            _args = argsList.ToArray();
            TestParseArgsFails(Program.ParseArgsException.ParseErrorCode.DUPLICATE_FLAG);
        }

        private void TestInputFlagRequired(int index)
        {
            List<string> argsList = _args.ToList();
            argsList.RemoveRange(index, 2);
            _args = argsList.ToArray();
            TestParseArgsFails(Program.ParseArgsException.ParseErrorCode.REQUIRED_FLAG_MISSING);
        }

        [TestInitialize]
        public void Initialize()
        {
            _args = new string[] { "-i", "inFile", "-o", "outDir", "-l", "Cpp" };
        }

        [TestMethod]
        public void TestDefaultValid()
        {
            ValidateParseArgs();
        }

        [TestMethod]
        public void TestInvalidArgCount()
        {
            List<string> argsList = _args.ToList();
            argsList.Add("foo");
            _args = argsList.ToArray();
            TestParseArgsFails(Program.ParseArgsException.ParseErrorCode.INVALID_ARG_COUNT);
        }

        [TestMethod]
        public void TestInvalidFlag()
        {
            List<string> argsList = _args.ToList();
            argsList.Add("--invalid");
            argsList.Add("foo");
            _args = argsList.ToArray();
            TestParseArgsFails(Program.ParseArgsException.ParseErrorCode.INVALID_FLAG);
        }

        [TestMethod]
        public void TestInputFileFlag()
        {
            TestInputFlag(INPUT_FILE_FLAG_INDEX, new string[] { "-i", "--input" });
        }

        [TestMethod]
        public void TestInputFileFlagRequired()
        {
            TestInputFlagRequired(INPUT_FILE_FLAG_INDEX);
        }

        [TestMethod]
        public void TestOutputDirFlag()
        {
            TestInputFlag(OUTPUT_DIR_FLAG_INDEX, new string[] { "-o", "--outdir" });
        }

        [TestMethod]
        public void TestOutputDirFlagRequired()
        {
            TestInputFlagRequired(OUTPUT_DIR_FLAG_INDEX);
        }

        [TestMethod]
        public void TestLangFlag()
        {
            TestInputFlag(LANG_FLAG_INDEX, new string[] { "-l", "--lang" });
        }

        [TestMethod]
        public void TestLangFlagRequired()
        {
            TestInputFlagRequired(LANG_FLAG_INDEX);
        }

        [TestMethod]
        public void TestLangStrings()
        {
            //test all valid lang strings
            string[] validLangStrs = { "Cpp" };
            foreach (string validLangStr in validLangStrs)
            {
                _args[LANG_FLAG_INDEX + 1] = validLangStr;
                ValidateParseArgs();
            }

            //test for invalid lang string
            _args[LANG_FLAG_INDEX + 1] = "invalidLang";
            TestParseArgsFails(Program.ParseArgsException.ParseErrorCode.INVALID_LANG_STRING);
        }
    }
}