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

        public virtual string Job { get { return typeof(Employee).Name; } }

        protected Project Project { get; set; }
        public uint? ProjectID { get { return Project?.ID; } }

        public Employee(string firstName, string secondName, Qualification qualification)
        {
            FirstName = firstName;
            SecondName = secondName;
            Qualification = qualification;
            Project = null;
        }

        public void AddProject(Project project)
        {
            Project = project;
        }

        public virtual void EndProject()
        {
            Project = null;
        }
    }
}
