using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewIDC.Projects
{
    public static class ProjectConfigService
    {
        public static string folderProject = Directory.GetCurrentDirectory() + "\\Orig\\PINI\\";
        public static string outputFolder = Directory.GetCurrentDirectory() + "\\Orig\\Output\\";
        public static string GetFileProjectConfig(int projectId)
        {
            return folderProject + "Project_" + projectId + ".ini";
        }
        public static string GetOutputFolder(int projectId) {
            return outputFolder + "output_" + projectId + ".xlsx";
        }
        public static List<ProjectConfigException> errors = new List<ProjectConfigException>();
    }
}
