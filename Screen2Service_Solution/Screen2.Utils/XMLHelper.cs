using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Screen2.Utils
{
    public class XMLHelper
    {
        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="inputObject">Object that is to be serialized to XML</param>
        /// <param name="type">The type of the input object.</param>
        /// <returns>XML string</returns>
        public static String SerializeObject<T>(T value)
        {
            //string xmlizedString = null;
            //XmlDocument xmlDoc;

            //XmlSerializer xs = new XmlSerializer(typeof(T));
            //System.IO.MemoryStream memStream = new MemoryStream();
            //StreamWriter strmWriter = new StreamWriter(memStream);
            //xs.Serialize(strmWriter, inputObject);

            //memStream.Position = 0;

            //xmlDoc = new XmlDocument();

            //xmlDoc.Load(memStream);
            //xmlizedString = xmlDoc.InnerXml;

            //return xmlizedString;

            if (value == null)
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }

        }

        /// <summary>
        /// Method to reconstruct an Object from XML string.
        /// </summary>
        /// <param name="xmlizedString">Input xml string to be deserialized.</param>
        /// <param name="type">The type of the deserialized object.</param>
        /// <returns>Deserialized object.</returns>
        public static T DeserializeObject<T>(String xml)
        {
            //T outputObject;

            //XmlSerializer xs = new XmlSerializer(typeof(T));

            //System.IO.StringReader reader = new StringReader(xmlizedString);

            //XmlTextReader xmlReader = new XmlTextReader(reader);

            //outputObject =(T)xs.Deserialize(xmlReader);

            //return outputObject;
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlReaderSettings settings = new XmlReaderSettings();
            // No settings need modifying here

            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        /// <summary>
        /// Transforms the specified XML data.
        /// </summary>
        /// <param name="xmlData">The XML data.</param>
        /// <param name="xslFileName">Name of the XSL file.</param>
        /// <returns></returns>
        public static string Transform(string xmlData, string xslFileName)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlData);

            return Transform(doc, xslFileName);
        }

        /// <summary>
        /// Transforms the specified XML input.
        /// </summary>
        /// <param name="xmlInput">The XML input.</param>
        /// <param name="xslFileName">Name of the XSL file.</param>
        /// <returns>the Output String</returns>
        public static string Transform(IXPathNavigable xmlInput, string xslFileName)
        {
           
            string outputString = string.Empty;

            // Create output stream and string
            MemoryStream outputStream = new MemoryStream();
            XmlWriterSettings xwSettings = new XmlWriterSettings();
            xwSettings.ConformanceLevel = ConformanceLevel.Auto;

            // Create output writer
            XmlWriter outputXML = XmlWriter.Create(outputStream,xwSettings);
            
            XslCompiledTransform xsl = new XslCompiledTransform();
            XsltSettings setting = new XsltSettings();

            setting.EnableScript = true;

            // Load the xsl file
            xsl.Load(xslFileName, setting, null);

            // apply transformation
            xsl.Transform(xmlInput, outputXML);

            // extract the output
            outputStream.Position = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(outputStream);
            outputString = xmlDoc.InnerXml;

            return outputString;
        }

        /// <summary>
        /// Dictionaries to XML.
        /// </summary>
        /// <param name="dict">The dict.</param>
        /// <returns>The XMLDocument</returns>
        public static XmlDocument DictionaryToXML(Dictionary<string, string> dict, string rootName)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement element;
            XmlElement rootElement;

            rootElement = xmlDoc.CreateElement(rootName);

            xmlDoc.PrependChild(rootElement);


            foreach (string key in dict.Keys)
            {
                element =  xmlDoc.CreateElement(key);
                element.InnerText = dict[key];
                rootElement.AppendChild(element);
            }

            return xmlDoc;
        }
    }
}
