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
    [DataContract(Name = "SolutionArchitect")]
    public sealed class SolutionArchitect : Architect
    {
        public SolutionArchitect() { }

        public SolutionArchitect(string firstName, string secondName, Qualification qualification)
            : base(firstName, secondName, qualification) { }
        
        public override string Job { get { return typeof(SolutionArchitect).Name; } }
    }
}
