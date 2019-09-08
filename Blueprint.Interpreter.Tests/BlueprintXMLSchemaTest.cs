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
        private XmlNode _fileVariableNode;
        private XmlNode _fileFunctionNode;
        private XmlNode _fileClassNode;
        private XmlNode _fileClassContructorNode;
        private XmlNode _fileClassVariableNode;
        private XmlNode _fileClassFunctionNode;
        private XmlNode _fileClassPropertyNode;
        private XmlNode _fileClassInnerClassNode;

        [TestInitialize]
        public void Initialize()
        {
            _doc = new XmlDocument();
            _doc.Load(
                "C:/Users/mhoga/Documents/Projects/Blueprint/Blueprint.Interpreter.Tests/testBlueprint.xml");
            _doc.Schemas.Add(null,
                "C:/Users/mhoga/Documents/Projects/Blueprint/Blueprint.Interpreter/BlueprintXMLSchema.xsd");

            //init node refs
            _blueprintNode = _doc.LastChild;
            _fileNode = _blueprintNode.FirstChild.FirstChild; //Blueprint/Files/File

            XmlNode fileContentNode = _fileNode.FirstChild; //File/content
            _fileVariableNode = fileContentNode.ChildNodes[0].FirstChild; //content/Variables/Variable
            _fileFunctionNode = fileContentNode.ChildNodes[1].FirstChild; //content/Functions/Function
            _fileClassNode = fileContentNode.ChildNodes[2].FirstChild; //content/Classes/Class

            XmlNode fileClassContentNode = _fileClassNode.ChildNodes[1]; //Class/content
            _fileClassContructorNode = fileClassContentNode.ChildNodes[0]; //content/Constructor
            _fileClassVariableNode = fileClassContentNode.ChildNodes[1].FirstChild; //content/Variables/Variable
            _fileClassFunctionNode = fileClassContentNode.ChildNodes[2].FirstChild; //content/Functions/Function
            _fileClassPropertyNode = fileClassContentNode.ChildNodes[3].FirstChild; //content/Properties/Property
            _fileClassInnerClassNode = fileClassContentNode.ChildNodes[4].FirstChild; //content/InnerClasses/Class
        }

        #region Helpers
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

        private void TestNodeOccurences(XmlNode node, uint minOccurences, uint maxOccurences)
        {
            XmlNode parentNode = node.ParentNode;

            //remove all occurences of child
            foreach (XmlNode childNode in parentNode.SelectNodes(node.Name))
            {
                parentNode.RemoveChild(childNode);
            }

            if (minOccurences == 0)
            {
                //test if still valid with no occurences
                Assert.IsTrue(ValidateXml());
            }
            else
            {
                //test one less than min is invalid
                for (uint i = 0; i < minOccurences - 1; i++)
                {
                    parentNode.AppendChild(node.CloneNode(true));
                }
                Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());

                //test exactly min is valid
                parentNode.AppendChild(node.CloneNode(true));
                Assert.IsTrue(ValidateXml());
            }

            //NOTE: maxOccurences = 0 means unbounded
            if (maxOccurences == 0)
            {
                //hard to test unbounded, so just test if more than one occurence (the default) is valid
                for (uint i = minOccurences; i < 2; i++)
                {
                    parentNode.AppendChild(node.CloneNode(true));
                }
                Assert.IsTrue(ValidateXml());
            }
            else
            {
                //test exactly max is valid
                for (uint i = minOccurences; i < maxOccurences; i++)
                {
                    parentNode.AppendChild(node.CloneNode(true));
                }
                Assert.IsTrue(ValidateXml());

                //test one more than max is invalid
                parentNode.AppendChild(node.CloneNode(true));
                Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());
            }
        }
        #endregion

        #region General
        [TestMethod, TestCategory("General")]
        public void TestBlueprintFileValid()
        {
            Assert.IsTrue(ValidateXml());
        }
        #endregion

        #region Blueprint

        #endregion

        #region File
        [TestMethod, TestCategory("File")]
        public void TestBlueprintFilesOccurences()
        {
            TestNodeOccurences(_fileNode.ParentNode, 1, 1);
        }

        [TestMethod, TestCategory("File")]
        public void TestFilesFileOccurences()
        {
            TestNodeOccurences(_fileNode, 1, 0);
        }

        [TestMethod, TestCategory("File")]
        public void TestFileNameRequired()
        {
            TestAttributeRequired(_fileNode, "name");
        }

        [TestMethod, TestCategory("File")]
        public void TestFileContentOccurences()
        {
            TestNodeOccurences(_fileVariableNode.ParentNode, 0, 1);
        }

        #region FileVariable
        [TestMethod, TestCategory("File"), TestCategory("FileVariable")]
        public void TestFileVariablesOccurences()
        {
            TestNodeOccurences(_fileVariableNode.ParentNode, 0, 1);
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

        [TestMethod, TestCategory("File"), TestCategory("FileVariable")]
        public void TestFileVariableOccurences()
        {
            TestNodeOccurences(_fileVariableNode, 1, 0);
        }
        #endregion

        #region FileFunction
        [TestMethod, TestCategory("File"), TestCategory("FileFunction")]
        public void TestFileFunctionsOccurences()
        {
            TestNodeOccurences(_fileFunctionNode.ParentNode, 0, 1);
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

        [TestMethod, TestCategory("File"), TestCategory("FileFunction")]
        public void TestFileFunctionOccurences()
        {
            TestNodeOccurences(_fileFunctionNode, 1, 0);
        }
        #endregion

        #region FileClass
        [TestMethod, TestCategory("File")]
        public void TestFileClassesOccurences()
        {
            TestNodeOccurences(_fileClassNode.ParentNode, 0, 1);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass")]
        public void TestFileClassNameRequired()
        {
            TestAttributeRequired(_fileClassNode, "name");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass")]
        public void TestFileClassOccurences()
        {
            TestNodeOccurences(_fileClassNode, 1, 0);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass")]
        public void TestFileClassAccessModifierOccurences()
        {
            TestNodeOccurences(_fileClassNode.FirstChild, 0, 1);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass")]
        public void TestFileClassContentOccurences()
        {
            TestNodeOccurences(_fileClassVariableNode.ParentNode, 0, 1);
        }

        #region FileClassContructor
        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassContructor")]
        public void TestFileClassContructorParamsOptional()
        {
            TestAttributeOptional(_fileClassContructorNode, "params");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassContructor")]
        public void TestFileClassContructorOccurences()
        {
            TestNodeOccurences(_fileClassContructorNode, 0, 1);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassContructor")]
        public void TestFileClassConstructorAccessModifierOccurences()
        {
            TestNodeOccurences(_fileClassContructorNode.FirstChild, 0, 1);
        }
        #endregion

        #region FileClassVariable
        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassVariable")]
        public void TestFileClassVariablesOccurences()
        {
            TestNodeOccurences(_fileClassVariableNode.ParentNode, 0, 1);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassVariable")]
        public void TestFileClassVariableNameRequired()
        {
            TestAttributeRequired(_fileClassVariableNode, "name");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassVariable")]
        public void TestFileClassVariableTypeRequired()
        {
            TestAttributeRequired(_fileClassVariableNode, "type");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassVariable")]
        public void TestFileClassVariableOccurences()
        {
            TestNodeOccurences(_fileClassVariableNode, 1, 0);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassVariable")]
        public void TestFileClassVariableAccessModifierOccurences()
        {
            TestNodeOccurences(_fileClassVariableNode.FirstChild, 0, 1);
        }
        #endregion

        #region FileClassFunction
        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassFunction")]
        public void TestFileClassFunctionsOccurences()
        {
            TestNodeOccurences(_fileClassFunctionNode.ParentNode, 0, 1);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClassFunction")]
        public void TestFileClassFunctionNameRequired()
        {
            TestAttributeRequired(_fileClassFunctionNode, "name");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClassFunction")]
        public void TestFileClassFunctionReturnTypeRequired()
        {
            TestAttributeRequired(_fileClassFunctionNode, "returnType");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClassFunction")]
        public void TestFileClassFunctionParamsOptional()
        {
            TestAttributeOptional(_fileClassFunctionNode, "params");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassFunction")]
        public void TestFileClassFunctionOccurences()
        {
            TestNodeOccurences(_fileClassFunctionNode, 1, 0);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassFunction")]
        public void TestFileClassFunctionAccessModifierOccurences()
        {
            TestNodeOccurences(_fileClassFunctionNode.FirstChild, 0, 1);
        }
        #endregion

        #region FileClassProperty
        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassProperty")]
        public void TestFileClassPropertiesOccurences()
        {
            TestNodeOccurences(_fileClassPropertyNode.ParentNode, 0, 1);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassProperty")]
        public void TestFileClassPropertyNameRequired()
        {
            TestAttributeRequired(_fileClassPropertyNode, "name");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassProperty")]
        public void TestFileClassPropertyTypeRequired()
        {
            TestAttributeRequired(_fileClassPropertyNode, "type");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassProperty")]
        public void TestFileClassPropertyOccurences()
        {
            TestNodeOccurences(_fileClassPropertyNode, 1, 0);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassProperty")]
        public void TestFileClassPropertyAccessModifierOccurences()
        {
            TestNodeOccurences(_fileClassPropertyNode.FirstChild, 0, 1);
        }
        #endregion

        #region FileClassInnerClass
        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassInnerClass")]
        public void TestFileInnerClassesOccurences()
        {
            TestNodeOccurences(_fileClassInnerClassNode.ParentNode, 0, 1);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassInnerClass")]
        public void TestFileClassInnerClassNameRequired()
        {
            TestAttributeRequired(_fileClassInnerClassNode, "name");
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassInnerClass")]
        public void TestFileClassInnerClassOccurences()
        {
            TestNodeOccurences(_fileClassInnerClassNode, 1, 0);
        }

        [TestMethod, TestCategory("File"), TestCategory("FileClass"), TestCategory("FileClassInnerClass")]
        public void TestFileClassInnerClassContentOccurences()
        {
            TestNodeOccurences(_fileClassInnerClassNode.FirstChild, 0, 1);
        }
        #endregion

        #endregion
        #endregion

    }
}
