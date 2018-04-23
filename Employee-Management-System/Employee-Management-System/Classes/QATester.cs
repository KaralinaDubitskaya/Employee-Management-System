using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class QATester : Employee
    {
        public QATester(string firstName, string secondName, Qualification qualification)
            : base(firstName, secondName, qualification) { }

        public override string Job { get { return typeof(QATester).Name; } }
    }
}
