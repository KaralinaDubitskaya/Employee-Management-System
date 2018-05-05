using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    [Serializable]
    [DataContract]
    public sealed class Project 
    {
        [DataMember]
        public uint ID { get; set; }
        [DataMember]
        public string Name { get; set; }

        public Project(uint id, string name)
        {
            ID = id;
            Name = name;
        }

        public Project() { }
    }
}
