using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Employee_Management_System
{
    [Serializable]
    [XmlInclude(typeof(Manager))]
    [XmlInclude(typeof(QATester))]
    [XmlInclude(typeof(SoftwareEngineer))]
    [XmlInclude(typeof(SolutionArchitect))]
    [XmlInclude(typeof(SystemArchitect))]
    [DataContract]
    [KnownType(typeof(Manager))]
    [KnownType(typeof(QATester))]
    [KnownType(typeof(SoftwareEngineer))]
    [KnownType(typeof(Architect))]
    public abstract class Employee
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string SecondName { get; set; }
        [DataMember]
        public Qualification Qualification { get; set; }

        [DataMember]
        public virtual string Job { get { return typeof(Employee).Name; } private set { } }

        protected Project Project { get; set; }
        [DataMember]
        public uint? ProjectID { get { return Project?.ID; } private set { } }

        public Employee(string firstName, string secondName, Qualification qualification)
        {
            FirstName = firstName;
            SecondName = secondName;
            Qualification = qualification;
            Project = null;
        }

        public Employee() { }

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
