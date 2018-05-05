using Microsoft.Win32;
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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Reflection;
using PluginContracts;
using Geffe;

namespace Employee_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Project> Projects { get; private set; }
        public List<Employee> Employees { get; private set; }

        private Project _selectedProject;
        private Employee _selectedEmployee;

        public delegate void DeleteProjectDelegate(uint id);
        public event DeleteProjectDelegate ProjectDeletedHandler;
        public delegate void AddEmployeeDelegate(Employee employee);
        private AddEmployeeDelegate _AddEmployeeHandler;

        private List<IPlugin> _plugins;

        public MainWindow()
        {
            InitializeComponent();

            Projects = new List<Project>();
            Employees = new List<Employee>();

            ProjectDeletedHandler += DeleteProjectFromProjects;
            ProjectDeletedHandler += DeleteProjectFromEmployees;

            _AddEmployeeHandler = AddEmployee;
            _selectedProject = null;
            _plugins = PluginLoader.LoadPlugins(Directory.GetCurrentDirectory() + @"\Plugins\");

            cbPlugins.Items.Add("None");
            cbPlugins.SelectedIndex = 0;
            if (_plugins != null)
            {
                foreach (IPlugin plugin in _plugins)
                {
                     cbPlugins.Items.Add(plugin);
                }
            }

            RefreshDataGrids();
        }

        private void RefreshDataGrids()
        {
            // Projects
            dgProjects.ItemsSource = null;
            dgProjects.ItemsSource = Projects;

            // Employees
            dgEmployees.ItemsSource = null;
            dgEmployees.ItemsSource = Employees;
        }
        private void UpdateProjectsList()
        {
            foreach (Employee employee in Employees)
            {
                Project project = employee.GetProject();
                if (project != null)
                {
                    int index = Projects.FindIndex(item => item.ID == project.ID);
                    if (index < 0)
                    {
                        Projects.Add(project);
                    }
                }
            }
        }

        #region Add/Delete project
        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            // Create and add new project
            AddProject(tbAddProject.Text);

            // Refresh the datagrid
            dgProjects.ItemsSource = null; 
            dgProjects.ItemsSource = Projects;

            // Clear the textbox
            tbAddProject.Text = "";
        }

        // Create and add new project
        private void AddProject(string name)
        {
            if (name?.Length == 0) { return; }
            uint id = GetProjectID(Projects);
            Project project = new Project(id, name);
            Projects.Add(project);
        }

        // Return unique ID for the new item of the list
        private uint GetProjectID(List<Project> projects)
        {
            uint id = (uint)projects.Count() - 1;

            bool isUnique;
            do
            {
                id++;
                isUnique = true;
                foreach (Project project in projects)
                {
                    if (project.ID == id)
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
            try
            {
                ProjectDeletedHandler?.Invoke(_selectedProject.ID);
            }
            catch (Exception)
            {
                MessageBox.Show("Please, choose the project.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Refresh the datagrid
            RefreshDataGrids();
        }

        // Delete project from projects list by id
        private void DeleteProjectFromProjects(uint id)
        {
            foreach (Project project in Projects)
            {
                if (project.ID == id)
                {
                    Projects.Remove(project);
                    break;
                }
            }
        }

        // Clear project property of the employees
        private void DeleteProjectFromEmployees(uint id)
        {
            foreach (Employee employee in Employees)
            {
                if (employee.ProjectID == id)
                {
                    employee.EndProject();
                    break;
                }
            }
        }
        #endregion      

        #region Add/Edit/Delete Employee
        #region AddEmployee
        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEmployee form = new AddEmployee(Projects, null, _AddEmployeeHandler);
                form.ShowDialog();
                RefreshDataGrids();
            }
            catch (Exception) { }
        }

        private void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
        }
        #endregion

        #region Edit/Delete Employee
        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEmployee form = new AddEmployee(Projects, _selectedEmployee, _AddEmployeeHandler);
                form.ShowDialog();
                Employees.Remove(_selectedEmployee);
                RefreshDataGrids();
            }
            catch (Exception) { }
        }

        private void dgProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedProject = dgProjects.SelectedItem as Project;
        }

        private void dgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedEmployee = dgEmployees.SelectedItem as Employee;
        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employees.Remove(_selectedEmployee);
            RefreshDataGrids();
        }
        #endregion
        #endregion

        #region Serialization
        #region Binary serialization
        private void btnBinSerialize_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlgSaveFile = new SaveFileDialog();
            BinaryFormatter serializer = new BinaryFormatter();

            dlgSaveFile.Filter = "dat files (*.dat)|*.dat";

            if (dlgSaveFile.ShowDialog() == true && dlgSaveFile.FileName != "")
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, Employees);
                        byte[] buffer = EncryptStream(stream);
                        using (FileStream st = File.Open(dlgSaveFile.FileName, FileMode.Create))
                        {
                            st.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot serialize data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnBinDeserialize_Click(object sender, RoutedEventArgs e)
        {
            Stream stream = null;
            OpenFileDialog dlgOpenFile = new OpenFileDialog();
            BinaryFormatter serializer = new BinaryFormatter();

            dlgOpenFile.Filter = "dat files (*.dat)|*.dat";

            if (dlgOpenFile.ShowDialog() == true)
            {
                try
                {
                    if ((stream = dlgOpenFile.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            byte[] buffer = DecryptStream(stream);
                            using (MemoryStream st = new MemoryStream(buffer))
                            {
                                Employees.Clear();
                                Employees = (List<Employee>)serializer.Deserialize(st);
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot deserialize data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            UpdateProjectsList();
            RefreshDataGrids();
        }
        #endregion

        #region XML serialization
        private void btnXmlSerialize_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlgSaveFile = new SaveFileDialog();

            Type tEmployee = typeof(Employee);
            Type[] jobs = Assembly.GetAssembly(tEmployee).GetTypes()
                .Where(type => (type.IsSubclassOf(tEmployee) && !type.IsAbstract)).ToArray();

            DataContractSerializer serializer = new DataContractSerializer(typeof(List<Employee>), jobs);

            dlgSaveFile.Filter = "xml files (*.xml)|*.xml";

            if (dlgSaveFile.ShowDialog() == true && dlgSaveFile.FileName != "")
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.WriteObject(stream, Employees);
                        byte[] buffer = EncryptStream(stream);
                        using (FileStream st = File.Open(dlgSaveFile.FileName, FileMode.Create))
                        {
                            st.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot serialize data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnXmlDeserialize_Click(object sender, RoutedEventArgs e)
        {
            Stream stream = null;
            OpenFileDialog dlgOpenFile = new OpenFileDialog();
            DataContractSerializer serializer = new DataContractSerializer(typeof(List<Employee>));

            dlgOpenFile.Filter = "xml files (*.xml)|*.xml";

            if (dlgOpenFile.ShowDialog() == true)
            {
                try
                {
                    if ((stream = dlgOpenFile.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            byte[] buffer = DecryptStream(stream);
                            using (MemoryStream st = new MemoryStream(buffer))
                            {
                                Employees.Clear();
                                Employees = (List<Employee>)serializer.ReadObject(st);
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot deserialize data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            UpdateProjectsList();
            RefreshDataGrids();
        }
        #endregion

        #region JSON serialization
        private void btnJSONSerialize_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlgSaveFile = new SaveFileDialog();

            Type tEmployee = typeof(Employee);
            Type[] jobs = Assembly.GetAssembly(tEmployee).GetTypes()
                .Where(type => (type.IsSubclassOf(tEmployee) && !type.IsAbstract)).ToArray();

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Employee>), jobs);

            dlgSaveFile.Filter = "json files (*.json)|*.json";

            if (dlgSaveFile.ShowDialog() == true && dlgSaveFile.FileName != "")
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.WriteObject(stream, Employees);
                        byte[] buffer = EncryptStream(stream);
                        using (FileStream st = File.Open(dlgSaveFile.FileName, FileMode.Create))
                        {
                            st.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot serialize data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnJSONDeserialize_Click(object sender, RoutedEventArgs e)
        {
            Stream stream = null;
            OpenFileDialog dlgOpenFile = new OpenFileDialog();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Employee>));

            dlgOpenFile.Filter = "json files (*.json)|*.json";

            if (dlgOpenFile.ShowDialog() == true)
            {
                try
                {
                    if ((stream = dlgOpenFile.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            byte[] buffer = DecryptStream(stream);
                            using (MemoryStream st = new MemoryStream(buffer))
                            {
                                Employees.Clear();
                                Employees = (List<Employee>)serializer.ReadObject(st);
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot deserialize data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            UpdateProjectsList();
            RefreshDataGrids();
        }
        #endregion
        #endregion

        #region Encrypt/Decrypt Stream
        private byte[] EncryptStream(MemoryStream stream)
        {
            byte[] buffer = stream.ToArray();
            //if (cbPlugins.SelectedIndex != 0)
            //{
            //    IPlugin plugin = cbPlugins.SelectedItem as IPlugin;
            //    //IPlugin plugin = Activator.CreateInstance(typeof()) as IPlugin;
            //    plugin.Encrypt(buffer);
            //}
            //IPlugin plugin = Activator.CreateInstance(typeof(Geffe.Geffe)) as IPlugin;
            //plugin.Encrypt(buffer);
            return buffer;
        }

        private byte[] DecryptStream(Stream stream)
        {
            byte[] buffer = StreamToArray(stream);
            //if (cbPlugins.SelectedIndex != 0)
            //{
            //    var pl = _plugins[cbPlugins.SelectedIndex - 1];
            //    Type type = pl.GetType();
            //    IPlugin plugin = Activator.CreateInstance(type) as IPlugin;
            //    plugin.Decrypt(buffer);
            //}
            //IPlugin plugin = Activator.CreateInstance(typeof(Geffe.Geffe)) as IPlugin;
            //plugin.Decrypt(buffer);
            return buffer;
        }

        private byte[] StreamToArray(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion
    }
}
