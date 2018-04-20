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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Employee_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Project> projects;
        private List<Employee> employees;

        public delegate void DeleteItem(uint id);
        public event DeleteItem ProjectDeletedHandler;

        private int selectedProject;

        public MainWindow()
        {
            InitializeComponent();

            projects = new List<Project>();
            employees = new List<Employee>();

            ProjectDeletedHandler += DeleteProjectFromProjects;
            ProjectDeletedHandler += DeleteProjectFromEmployees;

            selectedProject = 0;
        }

        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            // Create and add new project
            AddProject(tbAddProject.Text);

            // Refresh the datagrid
            dgProjects.ItemsSource = null; 
            dgProjects.ItemsSource = projects;

            // Clear the textbox
            tbAddProject.Text = "";
        }

        // Create and add new project
        private void AddProject(string name)
        {
            uint id = GetUniqueID(projects.ToArray<IIdentifiable>());
            Project project = new Project(id, name);
            projects.Add(project);
        }

        // Return unique ID for the new item of the list
        private uint GetUniqueID(IIdentifiable[] list)
        {
            uint id = (uint)projects.Count() - 1;

            bool isUnique;
            do
            {
                id++;
                isUnique = true;
                foreach (IIdentifiable elem in list)
                {
                    if (elem.ID == id)
                    {
                        isUnique = false;
                        break;
                    }
                }
            }
            while (!isUnique);

            return id;
        }

        private void btnDeleteProject_Click(object sender, RoutedEventArgs e)
        {
            // Delete the project
            uint id = Convert.ToUInt32(tbDeleteProject.Text);
            ProjectDeletedHandler?.Invoke(id);

            // Refresh the datagrid
            RefreshDataGrids();

            // Clear the textbox
            tbDeleteProject.Text = "";
        }

        // Delete project from projects list by id
        private void DeleteProjectFromProjects(uint id)
        {
            foreach (Project project in projects)
            {
                if (project.ID == id)
                {
                    projects.Remove(project);
                    break;
                }
            }
        }

        // Clear project property of the employees
        private void DeleteProjectFromEmployees(uint id)
        {
            foreach (Employee employee in employees)
            {
                if (employee.ProjectID == id)
                {
                    employee.EndProject();
                    break;
                }
            }
        }

        private void RefreshDataGrids()
        {
            // Projects
            dgProjects.ItemsSource = null;
            dgProjects.ItemsSource = projects;

            // Employees
            dgEmployees.ItemsSource = null;
            dgEmployees.ItemsSource = employees;

            // Tasks
            dgTasks.ItemsSource = null;
            Project selectedProject = dgProjects.SelectedItem as Project;
            dgTasks.ItemsSource = selectedProject?.GetTasks();
        }
    }
}
