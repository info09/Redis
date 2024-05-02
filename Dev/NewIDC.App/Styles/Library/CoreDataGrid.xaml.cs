using NewIDC.App.Models.LibraryModels;
using NewIDC.App.Styles.Library.LibraryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace NewIDC.App.Styles.Library
{
    /// <summary>
    /// Interaction logic for CoreDataGrid.xaml
    /// </summary>
    public partial class CoreDataGrid : UserControl
    {
        public CoreDataGrid()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        #region Dependency Registry
        public static DependencyProperty DataGridItemsProperty = DependencyProperty.Register("DataGridItems", typeof(ICollection), typeof(CoreDataGrid), new PropertyMetadata(null));
        public ICollection DataGridItems
        {
            get => (ICollection)GetValue(DataGridItemsProperty);
            set
            {
                SetValue(DataGridItemsProperty, value);
                mainDataGrid.ItemsSource = dataProcessedForDisplayed().DefaultView;

            }
        }
        public static DependencyProperty HeaderItemsProperty = DependencyProperty.Register("HeaderItems", typeof(HeaderInput), typeof(CoreDataGrid), new PropertyMetadata(null));
        public HeaderInput HeaderItems
        {
            get => (HeaderInput)GetValue(HeaderItemsProperty);
            set => SetValue(HeaderItemsProperty, value);
        }
        private DataTable _gridDataProcessedItems { get; set; }
        #endregion
        private DataTable dataProcessedForDisplayed()
        {
            JArray inputData = JArray.Parse(JsonConvert.SerializeObject(DataGridItems));
            DataTable table = new DataTable();
            AddColumnToDataTable(table, inputData);
            AddHeaderLine(table);
            AddColumnHeaderRow(table, inputData);
            AddDataToDataTable(table, inputData);
            return table;
        }
        #region DataTable Processing
        private void AddColumnToDataTable(DataTable Table, JArray InputData)
        {
            Table.Columns.Add("RowHeader", typeof(CellDataInput));
            for (int i = 1; i <= InputData[0].Count(); i++)
            {
                Table.Columns.Add(DataGridServices.GetHeaderName(i), typeof(CellDataInput));
            }
        }
        private void AddHeaderLine(DataTable dataTable)
        {
            bool isCbo = HeaderItems.HeaderType == HeaderInputType.Combobox;
            AddHeaderLine_Header(dataTable, isCbo);
            AddHeaderLine_RowHeader(dataTable, isCbo);
        }
        private void AddHeaderLine_Header(DataTable dataTable, bool isCombobox)
        {
            if (HeaderItems.HeaderType == HeaderInputType.None)
                return;
            int idx = 1;
            DataRow headerRow = dataTable.NewRow();
            DataRow textBelowComboboxHeader = dataTable.NewRow();
            foreach (HeaderInputData item in HeaderItems.Data)
            {
                string headerName = DataGridServices.GetHeaderName(item.ColumnIndex);
                switch (HeaderItems.HeaderType)
                {
                    case HeaderInputType.Combobox:
                        ComboboxHeaderInput comboboxData = (ComboboxHeaderInput)item;
                        headerRow[headerName] = CellDataInput.ComboboxDisplayed(comboboxData.Items, comboboxData.SelectedValue);
                        textBelowComboboxHeader[headerName] = CellDataInput.TextBlockDisplayed(comboboxData.TextBelow, Brushes.Black, Brushes.White, TextAlignment.Left, FontWeights.Normal);
                        break;
                    case HeaderInputType.Checkbox:
                        CheckboxHeaderInput checkboxData = (CheckboxHeaderInput)item;
                        headerRow[headerName] = CellDataInput.CheckboxDisplayed(checkboxData.IsChecked, checkboxData.CheckboxText);
                        break;
                    case HeaderInputType.Textbox:
                        TextErrorHeaderInput textboxData = (TextErrorHeaderInput)item;
                        headerRow[headerName] = CellDataInput.TextBlockDisplayed(textboxData.Text, Brushes.Red, Brushes.White, TextAlignment.Center, FontWeights.Bold);
                        break;
                    case HeaderInputType.None:
                        break;
                }
                idx++;
            }
            dataTable.Rows.Add(headerRow);
            if (isCombobox)
                dataTable.Rows.Add(textBelowComboboxHeader);
        }
        private void AddHeaderLine_RowHeader(DataTable dataTable, bool isCombobox)
        {
            if (dataTable.Rows.Count == 0) return;
            int columnCount = dataTable.Rows[0].ItemArray.Count();
            for (int i = 0; i < columnCount; i++)
            {
                string columnName = i == 0 ? "RowHeader" : DataGridServices.GetHeaderName(i);
                if (columnName == "RowHeader" && isCombobox)
                {
                    dataTable.Rows[0][columnName] = CellDataInput.TextBlockDisplayed("各列操作", Brushes.Black, Brushes.White, TextAlignment.Left, FontWeights.Bold);
                    dataTable.Rows[1][columnName] = CellDataInput.TextBlockDisplayed("処理コード", Brushes.Black, Brushes.White, TextAlignment.Left, FontWeights.Bold);
                }
                if (dataTable.Rows[0][columnName].ToString() == string.Empty)
                    dataTable.Rows[0][columnName] = CellDataInput.EmptyDisplayed(Brushes.Black, Brushes.White);
                if (dataTable.Rows[1][columnName].ToString() == string.Empty && isCombobox)
                    dataTable.Rows[1][columnName] = CellDataInput.EmptyDisplayed(Brushes.Black, Brushes.White);

            }
        }
        private void AddColumnHeaderRow(DataTable dataTable, JArray InputData)
        {
            DataRow dr = dataTable.NewRow();
            DataRow drHeader = dataTable.NewRow();
            int colIndex = 1;
            foreach (JProperty prop in InputData[0])
            {
                string headerName = DataGridServices.GetHeaderName(colIndex);
                dr[headerName] = CellDataInput.TextBlockDisplayed(headerName, Brushes.White, (SolidColorBrush)new BrushConverter().ConvertFromInvariantString("#666666"), TextAlignment.Center, FontWeights.Bold);
                drHeader[headerName] = CellDataInput.TextBlockHeaderDisplayed(prop.Name);
                colIndex++;
            }
            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                if (dr[i].ToString() == string.Empty)
                    dr[i] = CellDataInput.EmptyDisplayed(Brushes.White, (SolidColorBrush)new BrushConverter().ConvertFromInvariantString("#666666"));
                if (drHeader[i].ToString() == string.Empty)
                    drHeader[i] = CellDataInput.TextBlockHeaderDisplayed("");

            }
            dataTable.Rows.Add(dr);
            dataTable.Rows.Add(drHeader);
        }
        private void AddDataToDataTable(DataTable dataTable, JArray InputData)
        {
            int rowIdx = 0;
            foreach (JObject item in InputData)
            {
                DataRow dr = dataTable.NewRow();

                int colIndex = 0;
                foreach (JProperty prop in item.Properties())
                {
                    if (rowIdx == 0)
                        continue;
                    string headerName = DataGridServices.GetHeaderName(colIndex);
                    if (colIndex == 0)
                    {
                        dr["RowHeader"] = CellDataInput.TextBlockDisplayed(rowIdx.ToString(), Brushes.White, (SolidColorBrush)new BrushConverter().ConvertFromInvariantString("#666666"), TextAlignment.Center, FontWeights.Bold);
                        colIndex++;
                        headerName = DataGridServices.GetHeaderName(colIndex);
                    }
                    dr[headerName] = CellDataInput.TextBlockDisplayed(prop.Value.ToString(), Brushes.Black, Brushes.White, TextAlignment.Left, FontWeights.Normal);
                    colIndex++;
                }
                if (rowIdx != 0)
                {
                    dataTable.Rows.Add(dr);
                }
                rowIdx++;
            }
        }
        #endregion
        private void mainDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataTemplate dt = null;
            if (e.PropertyType == typeof(CellDataInput))
                dt = (DataTemplate)Resources["CellTemplate"];

            if (dt != null)
            {
                DataGridTemplateColumn c = new DataGridTemplateColumn()
                {
                    CellTemplate = dt,
                    Header = e.Column.Header,
                    HeaderTemplate = e.Column.HeaderTemplate,
                    HeaderStringFormat = e.Column.HeaderStringFormat,
                    SortMemberPath = e.PropertyName,
                };
                e.Column = c;
            }
        }

    }
}
