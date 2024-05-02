using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace NewIDC.App.ViewModels
{
    public class DataGridVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICollection _dataSource {  get; set; }
        public ICollection DataSource 
        {
            get
            {
                return _dataSource;
            }
            set
            {
                value = DatasourceProcessing(value);
                _dataSource = value;
            }
        }



        public ICollection DatasourceProcessing(ICollection input)
        {
            try
            {
                if (input == null)
                    return null;
                JArray jobjectData = JArray.Parse(JsonConvert.SerializeObject(input));
                JArray convertedData = new JArray();
                JObject headerRow = new JObject();
                int headerRowIdx = 0;
                foreach (JProperty property in ((JObject)jobjectData[0]).Properties())
                {
                    headerRowIdx++;
                    headerRow.Add(GetHeaderName(headerRowIdx), property.Name);
                }
                convertedData.Add(headerRow);
                foreach(JObject item in jobjectData)
                {
                    int index = 0;
                    JObject rowData = new JObject();
                    foreach(JProperty property in item.Properties())
                    {
                        index++;
                        rowData.Add(GetHeaderName(index), property.Value);
                    }
                    convertedData.Add(rowData);
                }
                List<object> result = JsonConvert.DeserializeObject<List<object>>(convertedData.ToString());
                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error when processing datasource." + e.Message, "An error has occurred");
                return null;
            }
        }
        private string GetHeaderName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = string.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }
        public EventHandler<RoutedEventArgs> SelectedBorderChange { get; set; }
    }
}
