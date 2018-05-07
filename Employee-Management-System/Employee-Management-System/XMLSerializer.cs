using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class XMLSerializer : IXMLSerializer
    {
        private DataContractSerializer _serializer;

        public XMLSerializer(Type type)
        {
            _serializer = new DataContractSerializer(type);
        }

        public XMLSerializer(Type type, IEnumerable<Type> knownTypes)
        {
            _serializer = new DataContractSerializer(type, knownTypes);
        }

        public object ReadObject(Stream stream)
        {
            return _serializer.ReadObject(stream);
        }

        public void WriteObject(Stream stream, object obj)
        {
            _serializer.WriteObject(stream, obj);
        }
    }
}
