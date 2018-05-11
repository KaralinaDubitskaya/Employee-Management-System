using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Employee_Management_System
{
    public interface IXMLEncoder
    {
        XmlDocument Encode(Stream stream);
    }
}
