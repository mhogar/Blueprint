using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blueprint.Interpreter.Tests
{
    [TestClass]
    public class BlueprintXMLSchemaTest
    {
        private XmlDocument _doc;
        private XmlNode _blueprintNode;
        private XmlNode _fileNode;
        private XmlNode _fileContentNode;
        private XmlNode _fileVariableNode;
        private XmlNode _fileFunctionNode;
        private XmlNode _fileClassNode;
        private XmlNode _fileClassAccessModifierNode;
        private XmlNode _fileClassContentNode;

        [TestInitialize]
        public void Initialize()
        {
            _doc = new XmlDocument();
            _doc.Load(
                "C:/Users/mhoga/Documents/Projects/Blueprint/Blueprint.Interpreter.Tests/testBlueprint.xml");
            _doc.Schemas.Add(null,
                "C:/Users/mhoga/Documents/Projects/Blueprint/Blueprint.Interpreter/BlueprintXMLSchema.xsd");

            _blueprintNode = _doc.LastChild;
            _fileNode = _blueprintNode.FirstChild;
            _fileContentNode = _fileNode.FirstChild;
            _fileVariableNode = _fileContentNode.FirstChild;
            _fileFunctionNode = _fileContentNode.ChildNodes[1];
            _fileClassNode = _fileContentNode.ChildNodes[2];
            _fileClassAccessModifierNode = _fileClassNode.FirstChild;
            _fileClassContentNode = _fileClassNode.LastChild;
        }

        private bool ValidateXml()
        {
            _doc.Validate(new ValidationEventHandler((object sender, ValidationEventArgs args) =>
            {
                if (args.Severity == XmlSeverityType.Error)
                {
                    throw args.Exception;
                }
            }));

            return true;
        }

        private void TestAttributeRequired(XmlNode node, string attributeName)
        {
            //Remove the attribute and test invalid
            node.Attributes.Remove(node.Attributes[attributeName]);
            Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());
        }

        private void TestAttributeOptional(XmlNode node, string attributeName)
        {
            //Remove the attribute and test still valid
            node.Attributes.Remove(node.Attributes[attributeName]);
            Assert.IsTrue(ValidateXml());
        }

        private void TestNodeMinOccurences(XmlNode parentNode, XmlNode childNode, uint minOccurences)
        {
            if (minOccurences == 0) return;

            //remove all occurences of child
            foreach (XmlNode node in parentNode.SelectNodes(childNode.Name))
            {
                parentNode.RemoveChild(node);
            }

            //test one less than min is invalid
            for (uint i = 0; i < minOccurences - 1; i++)
            {
                parentNode.AppendChild(childNode.CloneNode(true));
            }
            Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());

            //test exactly min is valid
            parentNode.AppendChild(childNode.CloneNode(true));
            Assert.IsTrue(ValidateXml());
        }

        private void TestNodeMaxOccurences(XmlNode parentNode, XmlNode childNode, uint maxOccurences)
        {
            //NOTE: maxOccurences = 0 means unbounded
            if (maxOccurences == 0) return;

            //remove all occurences of child
            foreach (XmlNode node in parentNode.SelectNodes(childNode.Name))
            {
                parentNode.RemoveChild(node);
            }

            //test exactly max is valid
            for (uint i = 0; i < maxOccurences; i++)
            {
                parentNode.AppendChild(childNode.CloneNode(true));
            }
            Assert.IsTrue(ValidateXml());

            //test one more than max is invalid
            parentNode.AppendChild(childNode.CloneNode(true));
            Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());
        }

        [TestMethod, TestCategory("General")]
        public void TestBlueprintFileValid()
        {
            Assert.IsTrue(ValidateXml());
        }

        [TestMethod, TestCategory("Blueprint")]
        public void TestBlueprintFileMinOccurences()
        {
            TestNodeMinOccurences(_blueprintNode, _fileNode, 1);
        }

        [TestMethod, TestCategory("File")]
        public void TestFileNameRequired()
        {
            TestAttributeRequired(_fileNode, "name");
        }

        [TestMethod, TestCategory("File")]
        public void TestFileContentMaxOccurences()
        {
            TestNodeMaxOccurences(_fileNode, _fileContentNode, 1);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileVariable")]
        public void TestFileVariableNameRequired()
        {
            TestAttributeRequired(_fileVariableNode, "name");
        }
        
        [TestMethod, TestCategory("File"), TestCategory("FileVariable")]
        public void TestFileVariableTypeRequired()
        {
            TestAttributeRequired(_fileVariableNode, "type");
        }
        
        [TestMethod, TestCategory("File"), TestCategory("FileFunction")]
        public void TestFileFunctionNameRequired()
        {
            TestAttributeRequired(_fileFunctionNode, "name");
        }
        
        [TestMethod, TestCategory("File"), TestCategory("FileFunction")]
        public void TestFileFunctionReturnTypeRequired()
        {
            TestAttributeRequired(_fileFunctionNode, "returnType");
        }
        
        [TestMethod, TestCategory("File"), TestCategory("FileFunction")]
        public void TestFileFunctionParamsOptional()
        {
            TestAttributeOptional(_fileFunctionNode, "params");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass")]
        public void TestFileClassNameRequired()
        {
            TestAttributeRequired(_fileClassNode, "name");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass")]
        public void TestFileClassAccessModifierMaxOccurences()
        {
            TestNodeMaxOccurences(_fileClassNode, _fileClassAccessModifierNode, 1);
        }
        
        [TestMethod, TestCategory("File"), TestCategory("FileClass")]
        public void TestFileClassContentMaxOccurences()
        {
            TestNodeMaxOccurences(_fileClassNode, _fileClassContentNode, 1);
        }

    }
}
