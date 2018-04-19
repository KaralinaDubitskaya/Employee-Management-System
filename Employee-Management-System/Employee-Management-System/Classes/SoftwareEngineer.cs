using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class SoftwareEngineer: Employee
    {
        public SoftwareEngineer(string firstName, string secondName, Qualification qualification)
            : base(firstName, secondName, qualification) { }

        public string Technologies { get; set; }
        public string Platforms { get; set; }
        public string CSVs { get; set; }
    }
}
