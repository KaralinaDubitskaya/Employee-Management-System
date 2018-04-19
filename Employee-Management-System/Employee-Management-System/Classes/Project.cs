using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class Project
    {
        public uint ID { get; private set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; private set; }

        public Project(string name)
        {
            Name = name;
            Tasks = new List<Task>();
        }

        public void AddTask(uint id, string taskName)
        {

        }
    }
}
