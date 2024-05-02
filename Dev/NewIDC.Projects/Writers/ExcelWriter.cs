using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace NewIDC.Projects {
    public class ExcelWriter : IWriter {
        public void Write(string filePath, List<string[]> contents) {
            // Create a new Excel application
            Excel.Application excelApp = new Excel.Application();

            // Add a new workbook
            Excel.Workbook workbook = excelApp.Workbooks.Add();

            // Get the first worksheet
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];

            // Add some data to cells
            int rowIndex = 1;
            foreach (string[] row in contents) {
                for (int i = 0; i < row.Length; i++) {
                    worksheet.Cells[rowIndex, i + 1] = row[i];
                }
                rowIndex++;
            }

            // Save the workbook
            workbook.SaveAs(filePath);

            // Close the workbook and Excel application
            workbook.Close();
            excelApp.Quit();
        }
    }
}
