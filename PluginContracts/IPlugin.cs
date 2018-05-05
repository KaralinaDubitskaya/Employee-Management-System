using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginContracts
{
    public interface IPlugin
    {
        string Name { get; }

        void Encrypt(byte[] arr);
        void Decrypt(byte[] arr);
    }
}
