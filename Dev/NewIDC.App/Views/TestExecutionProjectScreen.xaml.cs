using NewIDC.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NewIDC.App.Views {
    /// <summary>
    /// Interaction logic for TestExecutionProjectScreen.xaml
    /// </summary>
    public partial class TestExecutionProjectScreen : Window {
        public ObservableCollection<ProjectConfig> ProjectList { get; set; }
        public ProjectConfig SelectedProject { get; set; }
        private ConversionExecution convertExecution;
        private IProjectRepository projectRepository;
        public TestExecutionProjectScreen(IProjectRepository projectRepository) {
            InitializeComponent();
            DataContext = this;
            this.projectRepository = projectRepository;
            ProjectList = new ObservableCollection<ProjectConfig>(projectRepository.GetProjects().OrderBy(p => p.ProjectId).ToList());
            convertExecution = new ConversionExecution(new ExcelWriter());
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            if (SelectedProject != null) {
                try {
                    convertExecution.Convert(SelectedProject);
                    MessageBox.Show("The project was executed successfully!", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (Exception ex){
                    MessageBox.Show("Execution failed!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a project !", "Waring", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            if (SelectedProject == null) {
                return;
            }
            this.projectRepository.DeleteProject(SelectedProject.ProjectId);
            ProjectList.Remove(SelectedProject);
        }
    }
    public class ProjectRepositoryDummy : IProjectRepository {
        public void AddProject(ProjectConfig project) {
            throw new NotImplementedException();
        }

        public void DeleteProject(int project_id) {
            throw new NotImplementedException();
        }

        public ProjectConfig GetProject(int project_id) {
            throw new NotImplementedException();
        }

        public List<ProjectConfig> GetProjects() {
            return new List<ProjectConfig> { 
                new ProjectConfig(){ProjectId = 1, ProjectName="Project 1", ProjectMemo = "p1", Status="Created"},
                new ProjectConfig(){ProjectId = 2, ProjectName="Project 2", ProjectMemo = "p2", Status="Created"},
                new ProjectConfig(){ProjectId = 3, ProjectName="Project 3", ProjectMemo = "p3", Status="Created"},
            };
        }

        public void UpdateProject(ProjectConfig project) {
            throw new NotImplementedException();
        }
    }
}
