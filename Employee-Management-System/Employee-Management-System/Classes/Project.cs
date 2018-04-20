using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class Project : IIdentifiable
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        private List<Task> Tasks { get; set; }

        public Project(uint id, string name)
        {
            ID = id;
            Name = name;
            Tasks = new List<Task>();
        }

        public List<Task> GetTasks()
        {
            return Tasks;
        }
    }
}
