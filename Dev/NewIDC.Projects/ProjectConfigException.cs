using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewIDC.Projects
{
    public class ProjectConfigException
    {
        public string message { get; set; }
        public ProjectConfigException(string erroMessage)
        {
            this.message = erroMessage;
        }
    }
}
