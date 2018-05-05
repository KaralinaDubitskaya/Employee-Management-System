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

            XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;

            foreach (XmlNode xn in nodes)
            {
                if (xn.Name != null)
                {
                    XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, xn.Attributes[0].Value, xmlDoc.DocumentElement.NamespaceURI);
                    node.InnerXml = xn.InnerXml;
                    XmlNode parent = xn.ParentNode;
                    parent.AppendChild(node);
                    parent.RemoveChild(xn);
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
                    XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, "Employee", xn.ParentNode.NamespaceURI);
                    XmlAttribute attr = xmlDoc.CreateAttribute("i", "type", xn.ParentNode.NamespaceURI);
                    attr.Value = xn.Name;
                    node.Attributes.Prepend(attr);
                    node.InnerXml = xn.InnerXml;
                    XmlNode parent = xn.ParentNode;
                    parent.AppendChild(node);
                    parent.RemoveChild(xn);
                }
            }
        }
    }
}
