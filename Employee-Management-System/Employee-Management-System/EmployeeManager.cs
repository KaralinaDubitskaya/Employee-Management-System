using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_System
{
    public class EmployeeManager : IEmployeeManager
    {
        private MainWindow.AddEmployeeDelegate _AddEmployeeDelegate;

        public EmployeeManager(MainWindow sender)
        {
            _AddEmployeeDelegate = sender.AddEmployeeHandler;
        }

        public void AddEmployee(Employee employee)
        {
            _AddEmployeeDelegate(employee);
        }
    }
}
