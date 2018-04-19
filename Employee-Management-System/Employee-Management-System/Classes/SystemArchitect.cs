using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class SystemArchitect : Architect
    {
        public SystemArchitect(string firstName, string secondName, Qualification qualification)
            : base(firstName, secondName, qualification) { }
    }
}
