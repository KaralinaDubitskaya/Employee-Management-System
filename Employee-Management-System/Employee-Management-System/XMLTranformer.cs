using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginContracts;
using System.Xml;
using System.Windows;

namespace Employee_Management_System
{
    public class XMLTransformer : IXMLSerializer
    {
        private XMLSerializer _serializer;

        public XMLTransformer(Type type)
        {
            _serializer = new XMLSerializer(type);
        }

        public XMLTransformer(Type type, IEnumerable<Type> knownTypes)
        {
            _serializer = new XMLSerializer(type, knownTypes);
        }

        public object ReadObject(Stream stream)
        {
            return _serializer.ReadObject(stream);
        }

        public void WriteObject(Stream stream, object obj)
        {
            _serializer.WriteObject(stream, obj);
        }

        // Tranform Xml before writing to file
        public XmlDocument EncodeXml(Stream stream, string pluginName, List<IPlugin> plugins)
        {
            // Prepare the stream for reading
            stream.Flush();
            stream.Position = 0;

            // Load Xml to XmlDocument
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);

            // Transform Xml
            IPlugin plugin = GetPlugin(pluginName, plugins);
            plugin?.Encode(ref xmlDoc);

            return xmlDoc;
        }

        // Tranform Xml after reading from file
        public MemoryStream DecodeXml(Stream fileStream, List<IPlugin> plugins)
        {
            // Load Xml from the stream
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileStream);

            string pluginName = xmlDoc.DocumentElement.Attributes["Plugin"]?.Value;
            if (pluginName == null)
            {
                // Write Xml to the stream
                MemoryStream xmlStream = new MemoryStream();
                xmlDoc.Save(xmlStream);
                xmlStream.Flush();
                xmlStream.Position = 0;

                return xmlStream;
            }

            // Tranform Xml
            IPlugin plugin = GetPlugin(pluginName, plugins);
            if (plugin != null)
            {
                plugin.Decode(ref xmlDoc);

                // Write Xml to the stream
                MemoryStream xmlStream = new MemoryStream();
                xmlDoc.Save(xmlStream);
                xmlStream.Flush();
                xmlStream.Position = 0;

                return xmlStream;
            }
            else
            {
                MessageBox.Show("Plugin not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new MemoryStream();
            }
        }

        // Return plugin selected by user 
        private IPlugin GetPlugin(string name, List<IPlugin> plugins)
        {
            if (name != "None" && name != null)
            {
                foreach (IPlugin plugin in plugins)
                {
                    if (name == plugin.Name)
                    {
                        return plugin;
                    }
                }
            }
            return null;
        }
    }
}
