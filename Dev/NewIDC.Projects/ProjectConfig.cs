using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NewIDC.Projects
{
    [Serializable]
    public class ProjectConfig
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectMemo { get; set; }

        public string SourceFilePath { get; set; }

        public string RecordConfigPath { get; set; }

        public string FileConfigPath { get; set; }

        public string OutputFilePath { get; set; }
        public List<ConversionBase> ConversionList { get; set; }

        public string Status { get; set; }

        public ConversionBase conversionBase { get; set; }

        public List<ConversionBase> GetConversionList()
        {
            return ConversionList;
        }
        public ProjectConfig()
        {
            ConversionList = new List<ConversionBase>();
        }
    }
}

