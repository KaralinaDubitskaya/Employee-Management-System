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
    [DataContract(Name = "Architect")]
    [KnownType(typeof(SolutionArchitect))]
    [KnownType(typeof(SystemArchitect))]
    public abstract class Architect : Employee
    {
        public Architect() { }

        public Architect(string firstName, string secondName, Qualification qualification)
            : base(firstName, secondName, qualification) { }
        
        public override string Job { get { return typeof(Architect).Name; } }
    }
}
