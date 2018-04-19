using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public abstract class Employee
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }     
        public Qualification Qualification { get; set; }

        protected Project Project { get; set; }
        protected Task Task { get; set; }
        
        public Employee(string firstName, string secondName, Qualification qualification)
        {
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.Qualification = qualification;
        }

        public virtual bool CompleteTask()
        {
            if (this.Task == null)
            {
                return false;
            }
            else
            {
                Task = null;
                return true;
            }
        }
    }
}
