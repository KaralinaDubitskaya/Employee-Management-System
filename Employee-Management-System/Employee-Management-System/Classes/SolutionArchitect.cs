using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System.Classes
{
    public class SolutionArchitect : Architect
    {
        public SolutionArchitect(string firstName, string secondName, Qualification qualification)
            : base(firstName, secondName, qualification) { }

        public override string Job { get { return typeof(SolutionArchitect).Name; } }
    }
}
