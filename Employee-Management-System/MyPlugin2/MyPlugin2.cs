using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginContracts;
using System.Xml;

namespace MyPlugin2 
{
    public class MyPlugin2 : IPlugin
    {
        public string Name { get { return "MyPlugin2"; } }

        public void Encode(ref XmlDocument xmlDoc)
        {
            XmlNode root = xmlDoc.DocumentElement;
            XmlAttribute pluginAttr = xmlDoc.CreateAttribute("Plugin");
            pluginAttr.Value = this.Name;
            root.Attributes.Append(pluginAttr);

            while (root.FirstChild.Name == "Employee")
            {
                XmlNode xn = root.FirstChild;
                XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, xn.Attributes[0].Value, xmlDoc.DocumentElement.NamespaceURI);
                node.InnerXml = xn.InnerXml;
                root.AppendChild(node);
                root.RemoveChild(xn);
            }
        }

        public void Decode(ref XmlDocument xmlDoc)
        {
            XmlNode root = xmlDoc.DocumentElement;

            while (root.FirstChild.Name != "Employee")
            {
                XmlNode xn = root.FirstChild;
                XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, "Employee", root.NamespaceURI);
                XmlAttribute attr = xmlDoc.CreateAttribute("i", "type", root.NamespaceURI);
                attr.Value = xn.Name;
                node.Attributes.Prepend(attr);
                node.InnerXml = xn.InnerXml;
                root.AppendChild(node);
                root.RemoveChild(xn);
            }
        }
    }
}
