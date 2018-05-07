using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public interface IXMLSerializer
    {
        object ReadObject(Stream stream);
        void WriteObject(Stream stream, Object obj);
    }
}
