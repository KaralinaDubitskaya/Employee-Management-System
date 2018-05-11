using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;

namespace Employee_Management_System
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        private Type[] _jobs;
        private List<Project> _projects;
        private IEmployeeManager _employeeManager;

        public AddEmployee(List<Project> projects, Employee employee)
        {
            InitializeComponent();
            
            _projects = projects;

            Type tEmployee = typeof(Employee);
            _jobs = Assembly.GetAssembly(tEmployee).GetTypes()
                .Where(type => (type.IsSubclassOf(tEmployee) && !type.IsAbstract)).ToArray();

            foreach (Type job in _jobs)
            {
                cbJob.Items.Add(job.Name);
            }

            foreach (var qualification in Enum.GetNames(typeof(Qualification)))
            {
                cbQualification.Items.Add(qualification);
            }

            cbProject.Items.Add("<-- None -->");
            foreach (Project project in _projects)
            {
                cbProject.Items.Add(project.ID);
            }

            if (employee == null)
            {
                tbFirstName.Text = "";
                tbSecondName.Text = "";
                cbJob.SelectedIndex = 0;
                cbQualification.SelectedIndex = 0;
                cbProject.SelectedIndex = 0;
            }
            else
            {
                tbFirstName.Text = employee.FirstName;
                tbSecondName.Text = employee.SecondName;
                cbJob.SelectedItem = employee.Job;
                cbQualification.SelectedIndex = (int)employee.Qualification;
                cbProject.SelectedItem = employee.ProjectID;
                if (employee.ProjectID == null)
                {
                    cbProject.SelectedIndex = 0;
                }
            }
        }

        public void SetEmployeeHandler(IEmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (tbFirstName.Text.Length == 0 || tbSecondName.Text.Length == 0)
            {
                MessageBox.Show("Please, fill all fields.", "Save changes", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Type job = _jobs[cbJob.SelectedIndex];
            string firstName = tbFirstName.Text;
            string secondName = tbSecondName.Text;
            Qualification qualification = (Qualification)cbQualification.SelectedIndex;

            dynamic employee = Activator.CreateInstance(job, firstName, secondName, qualification);
            if (cbProject.SelectedIndex != 0)
            {
                employee.AddProject(_projects[cbProject.SelectedIndex - 1]);
            }

            _employeeManager?.AddEmployee(employee);
            Close();
        }
    }
}
