using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginContracts;
using System.Xml;

namespace test
{
    public class MyPlugin : IPlugin
    {
        public string Name { get { return "MyPlugin"; } }

        public void Encode(ref XmlDocument xmlDoc)
        {
            XmlNode root = xmlDoc.DocumentElement;
            XmlAttribute pluginAttr = xmlDoc.CreateAttribute("Plugin");
            pluginAttr.Value = this.Name;
            root.Attributes.Append(pluginAttr);
        }

        public XmlDocument Decode(XmlDocument xmlDoc)
        {
            return xmlDoc;
        }
    }
}
