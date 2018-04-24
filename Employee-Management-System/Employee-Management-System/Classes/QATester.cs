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
    [DataContract]
    public sealed class QATester : Employee
    {
        public QATester() { }

        public QATester(string firstName, string secondName, Qualification qualification)
            : base(firstName, secondName, qualification) { }

        [DataMember]
        public override string Job { get { return typeof(QATester).Name; } }
    }
}
