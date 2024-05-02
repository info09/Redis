using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewIDC.Projects {
    public interface IWriter {
        void Write(string filePath, List<string[]> contents);
    }
}
