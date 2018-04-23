using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class Project 
    {
        public uint ID { get; set; }
        public string Name { get; set; }

        public Project(uint id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
