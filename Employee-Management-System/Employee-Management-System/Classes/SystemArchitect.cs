using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Employee_Management_System
{
    [Serializable]
    [DataContract(Name = "SystemArchitect")]
    public sealed class SystemArchitect : Architect
    {
        public SystemArchitect() { }

        public SystemArchitect(string firstName, string secondName, Qualification qualification)
            : base(firstName, secondName, qualification) { }

        [DataMember]
        public override string Job { get { return typeof(SystemArchitect).Name; } }
    }
}
