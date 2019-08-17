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

        [TestInitialize]
        public void Initialize()
        {
            _doc = new XmlDocument();
            _doc.Load(
                "C:/Users/mhoga/Documents/Projects/Blueprint/Blueprint.Interpreter.Tests/testBlueprint.xml");
            _doc.Schemas.Add(null,
                "C:/Users/mhoga/Documents/Projects/Blueprint/Blueprint.Interpreter/BlueprintXMLSchema.xsd");
        }

        public bool ValidateXml()
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

        [TestMethod, TestCategory("General")]
        public void TestBlueprintFileValid()
        {
            Assert.IsTrue(ValidateXml());
        }

        [TestMethod, TestCategory("General")]
        public void TestRootNotBlueprintInvalid()
        {
            //Remove blueprint node and add invalid node
            _doc.RemoveAll();
            _doc.AppendChild(_doc.CreateElement("NotBlueprint"));

            Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());
        }

        [TestMethod, TestCategory("Blueprint")]
        public void TestBlueprintNoFileInvalid()
        {
            //remove file node
            XmlNode node = _doc.LastChild;
            node.RemoveChild(node.SelectSingleNode("File"));

            Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());
        }

        [TestMethod, TestCategory("Blueprint")]
        public void TestBlueprintInvalidChild()
        {
            //remove file node
            XmlNode node = _doc.LastChild;
            node.AppendChild(_doc.CreateElement("NotFile"));

            Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());
        }

        [TestMethod, TestCategory("File")]
        public void TestFileNoNameInvalid()
        {
            //remove file's name
            XmlNode node = _doc.LastChild.FirstChild;
            node.Attributes.Remove(node.Attributes["name"]);

            Assert.ThrowsException<XmlSchemaValidationException>(() => ValidateXml());
        }
    }
}
