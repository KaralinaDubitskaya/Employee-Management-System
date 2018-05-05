using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PluginContracts;

namespace MyPlugin1
{
    // Transform node "Qualificaton" to attribute
    public class MyPlugin1 : IPlugin
    {
        public string Name { get { return "MyPlugin1"; } }

        public void Encode(ref XmlDocument xmlDoc)
        {
            XmlNode root = xmlDoc.DocumentElement;
            XmlAttribute pluginAttr = xmlDoc.CreateAttribute("Plugin");
            pluginAttr.Value = this.Name;
            root.Attributes.Append(pluginAttr);

            XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;

            foreach (XmlNode xn in nodes)
            {
                if (xn.Name == "Employee")
                {
                    XmlNode node = xn["Qualification"];
                    XmlAttribute attr = xmlDoc.CreateAttribute("Qualification");
                    attr.Value = node.InnerText;
                    xn.Attributes.Prepend(attr);
                    xn.RemoveChild(node);
                }
            }
        }

        public void Decode(ref XmlDocument xmlDoc)
        {
            XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;

            foreach (XmlNode xn in nodes)
            {
                if (xn.Name == "Employee")
                {
                    XmlAttribute attr = xn.Attributes["Qualification"];
                    XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, "Qualification", null);
                    node.InnerText = attr.Value;
                    xn.AppendChild(node);
                    xn.Attributes.Remove(attr);
                }
            }
        }
    }
}
