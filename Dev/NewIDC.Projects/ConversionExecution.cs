using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NewIDC.Projects
{
    public class ConversionExecution
    {
        private IProjectRepository iProjectRepository;
		private IWriter writer;
        public ConversionExecution(IWriter writer)
        {
            this.writer = writer;
        }
        public void Convert(ProjectConfig project)
		{
			ConversionBase previousConversion = new FirstConversion(project.SourceFilePath);
			var listConversion = project.GetConversionList().OrderBy(s => s.Order);
			foreach (ConversionBase conv in listConversion) {
				conv.SetPreviousConversion(previousConversion);
				previousConversion = conv;
			}
            List<string[]> convertResult = new List<string[]>();
            var convertContent = previousConversion.Convert();
            convertResult.Add(previousConversion.GetHeader());
            convertResult.AddRange(convertContent);
            if (!string.IsNullOrEmpty(project.OutputFilePath)) {
                string directoryPath = Path.GetDirectoryName(project.OutputFilePath);
                if (!Directory.Exists(directoryPath)) {
                    Directory.CreateDirectory(directoryPath);
                }
                writer.Write(project.OutputFilePath, convertResult);
            }
        }

	}

}

