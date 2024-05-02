using NewIDC.App.Models.LibraryModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace NewIDC.App.Styles.Library.LibraryServices
{
    public class DataGridServices
    {
        public static void ColoringRow_Changed(DataGrid dataGrid,List<ColoringRow> coloringRow)
        {
            int rowIndex = -1;
            foreach (var row in dataGrid.Items)
            {
                DataGridRow dataGridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(row);
                if (dataGridRow == null)
                    continue;
                rowIndex++;
                if (rowIndex == 0)
                    continue;
                foreach (var column in dataGrid.Columns)
                {
                    DataGridCell cell = GetCell(dataGrid, rowIndex, column.DisplayIndex);
                    if (cell == null)
                        continue;

                    var checkRow = coloringRow.FirstOrDefault(x => x.RowIndex == rowIndex);
                    if (checkRow != null)
                    {
                        cell.Background = checkRow.ColorBrush;
                        cell.BorderBrush = checkRow.ColorBrush;
                        continue;
                    }
                }
            }
        }

        public static void ColoringColumn_Changed(DataGrid dataGrid, List<ColoringColumn> coloringCol)
        {
            int rowIndex = -1;
            foreach (var row in dataGrid.Items)
            {
                DataGridRow dataGridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(row);
                if (dataGridRow == null)
                    continue;
                rowIndex++;
                if (rowIndex == 0)
                    continue;
                foreach (var column in dataGrid.Columns)
                {
                    DataGridCell cell = GetCell(dataGrid, rowIndex, column.DisplayIndex);
                    if (cell == null)
                        continue;

                    var checkColumn = coloringCol.FirstOrDefault(x => x.ColumnIndex == column.DisplayIndex);
                    if (checkColumn != null)
                    {
                        cell.Background = checkColumn.ColorBrush;
                        cell.BorderBrush = checkColumn.ColorBrush;
                        continue;
                    }
                }
            }
        }
        public static void GridData_LoadingRow(object sender, DataGridRowEventArgs e, int headerRowNumber)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            // Check if it's the first row
            if (e.Row.GetIndex() < headerRowNumber)
            {
                // Change background color for the first row
                e.Row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE599"));
                Style rowHeaderStyle = new Style(typeof(DataGridRowHeader));
                foreach (Setter setter in e.Row.HeaderStyle.Setters)
                {
                    Setter newSetter = new Setter();
                    newSetter.Property = setter.Property;
                    newSetter.Value = setter.Value;
                    if (setter.Property == Control.BackgroundProperty)
                    {
                        SolidColorBrush backgroundBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE599"));
                        newSetter.Value = backgroundBrush;
                    }
                    if (setter.Property == Control.ForegroundProperty)
                    {
                        SolidColorBrush foreBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
                        newSetter.Value = foreBrush;
                    }

                    rowHeaderStyle.Setters.Add(newSetter);
                }
                e.Row.HeaderStyle = rowHeaderStyle;
            }
        }
        public static ICollection DatasourceProcessing(ICollection input)
        {
            try
            {
                if (input == null)
                    return null;
                JArray jobjectData = JArray.Parse(JsonConvert.SerializeObject(input));
                JArray convertedData = new JArray();
                JObject headerRow = new JObject();
                int headerRowIdx = 0;
                var header = jobjectData[0];
                if (header.Type == JTokenType.Array) {
                    foreach (object value in header) {
                        headerRowIdx++;
                        headerRow.Add(GetHeaderName(headerRowIdx), value.ToString());
                    }
                } else {
                    foreach (JProperty property in ((JObject)jobjectData[0]).Properties()) {
                        headerRowIdx++;
                        headerRow.Add(GetHeaderName(headerRowIdx), property.Name);
                    }
                }

                convertedData.Add(headerRow);
                if (header.Type == JTokenType.Array) {
                    for (int i = 1; i< jobjectData.Count; i++) {
                        var row = jobjectData[i];
                        int index = 0;
                        JObject rowData = new JObject();
                        foreach (object value in row) {
                            index++;
                            rowData.Add(GetHeaderName(index), value.ToString());
                        }
                        convertedData.Add(rowData);
                    }
     
                } else {
                    foreach (JObject item in jobjectData) {
                        int index = 0;
                        JObject rowData = new JObject();
                        foreach (JProperty property in item.Properties()) {
                            index++;
                            rowData.Add(GetHeaderName(index), property.Value);
                        }
                        convertedData.Add(rowData);
                    }
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
        public static string GetHeaderName(int columnIndex)
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

        public static DataGridCell GetCell(DataGrid dataGrid, int rowIndex, int colIndex)
        {
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);

            if (row != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);

                if (presenter != null)
                {
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(colIndex);
                    return cell;
                }
            }

            return null;
        }
        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }
    }
    public class DataRowViewConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataGridCell cell = value as DataGridCell;
            if (cell == null)
                return null;

            System.Data.DataRowView drv = cell.DataContext as System.Data.DataRowView;
            if (drv == null)
                return null;

            return drv.Row[cell.Column.SortMemberPath];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
