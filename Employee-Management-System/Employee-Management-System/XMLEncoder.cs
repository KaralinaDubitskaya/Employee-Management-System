using PluginContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Employee_Management_System
{
    public class XMLEncoder : IXMLEncoder
    {
        private IPlugin _plugin;

        public XMLEncoder(IPlugin plugin)
        {
            _plugin = plugin;
        }

        public XmlDocument Encode(Stream stream)
        {
            // Load Xml to XmlDocument
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);

            // Transform Xml
            _plugin?.Encode(ref xmlDoc);

            return xmlDoc;
        }
    }
}
