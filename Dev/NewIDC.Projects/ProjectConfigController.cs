using System;

namespace NewIDC.Projects
{
    public class ProjectConfigController
    {
        public ProjectConfig project { get; set; }
        private IProjectRepository iProjectRepository;
        public EventHandler<EventArgs> PropertiesChanged;
        private static ProjectConfigController instance = new ProjectConfigController();
        public static ProjectConfigController GetInstance() { return instance; }
        private ProjectConfigController()
        {
            this.iProjectRepository = new IniProjectRepository();
        }
        public void OnProjectConfigChanged() {
            PropertiesChanged?.Invoke(this, new EventArgs());
        }
        public void AddConversion(ConversionBase conv)
        {
            this.project.ConversionList.Add(conv);
        }
        public void AddNewProjectConfig()
        {
            this.project = new ProjectConfig();
        }
        public void SaveConfig()
        {
            if (this.project.ProjectId != 0) {
                this.project.OutputFilePath = ProjectConfigService.GetOutputFolder(this.project.ProjectId);
                this.iProjectRepository.UpdateProject(this.project);
            } else {
                this.iProjectRepository.AddProject(this.project);
            }
        }
        public ProjectConfig GetProjectConfig() {
            return this.project;
        }
        public void SetProjectConfig(ProjectConfig projectConfig) {
            this.project = projectConfig;
        }
    }
}

