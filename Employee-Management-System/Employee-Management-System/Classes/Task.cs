using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class Task
    {
        public uint ID { get; private set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; private set; }

        public Task(uint id, string name)
        {
            ID = id;
            Name = name;
            Employees = new List<Employee>();
        }

        public void AddEmployee()
        {

        }
    }
}
