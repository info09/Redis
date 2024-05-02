using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Xml;
using System.Xml.Serialization;

namespace NewIDC.Projects
{
    public class IniProjectRepository : IProjectRepository
    {
        #region Method Get, Add, Get All ,Delete, Update

        /// <summary>
        /// Get ProjectConfig
        /// </summary>
        /// <param name="project_id"></param>
        /// <returns></returns>
        /// <exception cref="ProjectConfigException"></exception>
        public ProjectConfig GetProject(int project_id)
        {
            // Lấy đường dẫn file chứa ProjectConfig của project_id
            string fileProject = ProjectConfigService.GetFileProjectConfig(project_id);
            ProjectConfig projectConfig = new ProjectConfig();

            // Nếu tồn tại file
            if (File.Exists(fileProject))
            {
                //Đọc nội dung trong file
                string projectText = File.ReadAllText(fileProject);
                projectConfig = ConvertXMLToProjectConfig(projectText);
            }
            return projectConfig;
        }

        /// <summary>
        /// Add ProjectConfig
        /// </summary>
        /// <param name="project"></param>
        public void AddProject(ProjectConfig project)
        {
            project.ProjectId = GenCodeProject();
            //Tạo đường dẫn file để lưu chuỗi
            string projectConfigFile = ProjectConfigService.GetFileProjectConfig(project.ProjectId);
            // Chuyển đổi ProjectConfig object sang chuỗi XML, và lưu vào file
            ConvertProjectConfigToXMLAndSave(project, projectConfigFile);
        }

        /// <summary>
        /// Get All ProjectConfig
        /// </summary>
        /// <returns></returns>
        public List<ProjectConfig> GetProjects()
        {
            var projectList = new List<ProjectConfig>();
            DirectoryInfo d = new DirectoryInfo(ProjectConfigService.folderProject);
            FileInfo[] Files = d.GetFiles("Project_*.ini");
            foreach (FileInfo file in Files)
            {
                string projectText = File.ReadAllText(file.FullName); //Đọc file
                var projectConfig = ConvertXMLToProjectConfig(projectText);
                projectList.Add(projectConfig);
            }
            return projectList;
        }

        /// <summary>
        /// Delete ProjectConfig
        /// </summary>
        /// <param name="project_id"></param>
        public void DeleteProject(int project_id)
        {
            string fileProject = ProjectConfigService.GetFileProjectConfig(project_id);
            if (File.Exists(fileProject))
            {
                File.Delete(fileProject);
            }
        }

        /// <summary>
        /// Update ProjectConfig
        /// </summary>
        /// <param name="project"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void UpdateProject(ProjectConfig projectConfig)
        {
            string projectConfigFile = ProjectConfigService.GetFileProjectConfig(projectConfig.ProjectId);
            // Chuyển đổi ProjectConfig object sang chuỗi Json,Ghi nội dung chuỗi vào file
            ConvertProjectConfigToXMLAndSave(projectConfig, projectConfigFile);
        }
        #endregion

        #region Common

        /// <summary>
        /// Gen Id IDENTITY
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ProjectConfigException"></exception>
        public int GenCodeProject()
        {
            DirectoryInfo d = new DirectoryInfo(ProjectConfigService.folderProject);
            if (!Directory.Exists(d.FullName))
            {
                Directory.CreateDirectory(d.FullName);
            }
            FileInfo[] Files = d.GetFiles("Project_*.ini");
            int maxId = 0;
            foreach (FileInfo file in Files)
            {
                try
                {
                    int idProject = Int32.Parse(file.Name.Replace("Project_", "").Replace(".ini", ""));
                    if (idProject > maxId) maxId = idProject;
                }
                catch (Exception ex)
                {
                    ProjectConfigService.errors.Add(new ProjectConfigException(ex.Message));
                }
            }
            return maxId + 1;
        }

        /// <summary>
        /// Convert XML to ProjectConfig
        /// </summary>
        /// <param name="projectConfig"></param>
        /// <returns></returns>
        public ProjectConfig ConvertXMLToProjectConfig(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectConfig), new Type[] { typeof(ConversionBase) });
            object projectConfig = new ProjectConfig();
            try
            {
                using (StringReader reader = new StringReader(xml))
                {
                    projectConfig = serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                ProjectConfigService.errors.Add(new ProjectConfigException(ex.Message));
            }
            return (ProjectConfig)projectConfig;
        }

        /// <summary>
        /// Convert ProjectConfig to XML and save
        /// </summary>
        /// <param name="projectConfig"></param>
        /// <returns></returns>
        public void ConvertProjectConfigToXMLAndSave(ProjectConfig projectConfig, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectConfig), new Type[] { typeof(ConversionBase) });
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, projectConfig);
            }
        }
        #endregion
    }

}

