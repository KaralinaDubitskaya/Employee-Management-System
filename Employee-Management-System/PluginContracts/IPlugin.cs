using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PluginContracts
{
    public interface IPlugin
    {
        string Name { get; }

        void Encode(ref XmlDocument xmlDoc);
        void Decode(ref XmlDocument xmlDoc);
    }
}
