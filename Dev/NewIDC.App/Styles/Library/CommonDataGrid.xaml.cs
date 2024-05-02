using NewIDC.App.Models.LibraryModels;
using NewIDC.App.Styles.Library.LibraryServices;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NewIDC.App.Styles.Library
{
    /// <summary>
    /// Interaction logic for CommonDataGrid.xaml
    /// </summary>
    public partial class CommonDataGrid : UserControl
    {
        public static DependencyProperty ColoringRowProperty = DependencyProperty.Register("ColoringRow", typeof(List<ColoringRow>), typeof(CommonDataGrid), new PropertyMetadata(null));
        public List<ColoringRow> ColoringRow
        {
            get => (List<ColoringRow>)GetValue(ColoringRowProperty);
            set
            {
                SetValue(ColoringRowProperty, value);
                DataGridServices.ColoringRow_Changed(mainGridData,value);
            }
        }
        public static DependencyProperty ColoringColumnProperty = DependencyProperty.Register("ColoringColumn", typeof(List<ColoringColumn>), typeof(CommonDataGrid), new PropertyMetadata(null));
        public List<ColoringColumn> ColoringColumn
        {
            get => (List<ColoringColumn>)GetValue(ColoringColumnProperty);
            set
            {
                SetValue(ColoringColumnProperty, value);
                DataGridServices.ColoringColumn_Changed(mainGridData, value);
            }
        }
        public CommonDataGrid()
        {
            InitializeComponent();
            ZoomScale = 1;
        }
        public static DependencyProperty DataGridInputProperty = DependencyProperty.Register("DataGridInput", typeof(ICollection), typeof(CommonDataGrid), new FrameworkPropertyMetadata(null));
        public ICollection DataGridInput
        {
            get => (ICollection)GetValue(DataGridInputProperty);
            set
            {
                value = DataGridServices.DatasourceProcessing(value);
                SetValue(DataGridInputProperty, value);
            }
        }
        private ICollection _dataGridInput { get; set; }

        public static DependencyProperty HeaderSearchKeyProperty = DependencyProperty.Register("HeaderSearchKey", typeof(string), typeof(CommonDataGrid), new PropertyMetadata(null));
        public string HeaderSearchKey
        {
            get => (string)GetValue(HeaderSearchKeyProperty);
            set
            {
                SetValue(HeaderSearchKeyProperty, value);
                FindingHeader();
            }
        }
        private int _crntFoundHeaderIdx { get; set; }
        public static readonly DependencyProperty ZoomScaleProperty = DependencyProperty.Register("ZoomScale", typeof(double), typeof(CommonDataGrid), new FrameworkPropertyMetadata(null));

        public double ZoomScale
        {
            get { return (double)GetValue(ZoomScaleProperty); }
            set
            {
                if (value > 0.1 && value <= 2)
                    SetValue(ZoomScaleProperty, value);
            }
        }

        public static DependencyProperty HeaderRowNumberProperty = DependencyProperty.Register("HeaderRowNumber", typeof(int), typeof(CommonDataGrid), new FrameworkPropertyMetadata(1));
        public int HeaderRowNumber
        {
            get => (int)GetValue(HeaderRowNumberProperty);
            set => SetValue(HeaderRowNumberProperty, value);
        }



        private int _columnStartSelect { get; set; }
        public string ColumnIdxSelectedBegin
        {
            get
            {
                return _columnStartSelect.ToString();
            }
            set
            {
                int.TryParse(value, out int output);
                if (output == 0)
                    return;
                output--;
                _columnStartSelect = output;
                _columnEndSelect = _columnStartSelect + _columnSelectedNumber - 1;
                BorderChange();
            }
        }

        private int _columnSelectedNumber { get; set; }
        private int _columnEndSelect { get; set; }
        public string ColumnSelectedNumber
        {
            get
            {
                return _columnSelectedNumber.ToString();
            }
            set
            {
                int.TryParse(value, out int output);
                if (output == 0)
                    return;
                _columnSelectedNumber = output;
                _columnEndSelect = _columnStartSelect + _columnSelectedNumber - 1;
                BorderChange();
            }
        }

        private int _rowStartSelect { get; set; }
        public string RowIdxSelectedBegin
        {
            get
            {
                return _rowStartSelect.ToString();
            }
            set
            {
                int.TryParse(value, out int output);
                if (output == 0)
                    return;
                output--;
                _rowStartSelect = output;
                _rowEndSelect = _rowStartSelect + _rowSelectedNumber - 1;
                BorderChange();
            }
        }

        private int _rowSelectedNumber { get; set; }
        private int _rowEndSelect { get; set; }
        public string RowSelectedNumber
        {
            get
            {
                return _rowSelectedNumber.ToString();
            }
            set
            {
                int.TryParse(value, out int output);
                if (output == 0)
                    return;
                _rowSelectedNumber = output;
                _rowEndSelect = _rowStartSelect + _rowSelectedNumber - 1;
                BorderChange();
            }
        }
        private void mainGridData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridServices.GridData_LoadingRow(sender, e, HeaderRowNumber);
        }
        private void BorderChange()
        {
            int rowIndex = -1;
            foreach (var row in mainGridData.Items)
            {
                DataGridRow dataGridRow = (DataGridRow)mainGridData.ItemContainerGenerator.ContainerFromItem(row);
                if (dataGridRow == null)
                    continue;
                rowIndex++;
                foreach (var column in mainGridData.Columns)
                {
                    DataGridCell cell = DataGridServices.GetCell(mainGridData, rowIndex, column.DisplayIndex);
                    if (cell == null)
                        continue;
                    ChangeBorderCell(cell, column, rowIndex);
                }
            }
        }
        private void ChangeBorderCell(DataGridCell cell, DataGridColumn column, int rowIndex)
        {
            cell.BorderBrush = Brushes.Transparent;
            var cellStyle = new Style(typeof(DataGridCell));
            var thickness = new Thickness();
            bool isChangeBorder = false;

            bool isTopLeft = (rowIndex == _rowStartSelect && column.DisplayIndex == _columnStartSelect);
            bool isTopRight = (rowIndex == _rowStartSelect && column.DisplayIndex == _columnEndSelect);
            bool isBottomLeft = (rowIndex == _rowEndSelect && column.DisplayIndex == _columnStartSelect);
            bool isBottomRight = (rowIndex == _rowEndSelect && column.DisplayIndex == _columnEndSelect);

            bool isTop = (rowIndex == _rowStartSelect
                            && (column.DisplayIndex > _columnStartSelect
                                && column.DisplayIndex < _columnEndSelect));
            bool isBottom = (rowIndex == _rowEndSelect
                            && (column.DisplayIndex > _columnStartSelect
                                && column.DisplayIndex < _columnEndSelect));
            bool isLeft = (column.DisplayIndex == _columnStartSelect
                            && (rowIndex > _rowStartSelect
                                && rowIndex < _rowEndSelect));
            bool isRight = (column.DisplayIndex == _columnEndSelect
                            && (rowIndex > _rowStartSelect
                                && rowIndex < _rowEndSelect));



            if (isTopLeft)
            {
                thickness.Top = 2;
                thickness.Left = 2;
                isChangeBorder = true;
            }
            if (isTopRight)
            {
                thickness.Top = 2;
                thickness.Right = 2;
                isChangeBorder = true;
            }
            if (isBottomLeft)
            {
                thickness.Bottom = 2;
                thickness.Left = 2;
                isChangeBorder = true;
            }
            if (isBottomRight)
            {
                thickness.Bottom = 2;
                thickness.Right = 2;
                isChangeBorder = true;
            }
            if (isTop)
            {
                thickness.Top = 2;
                isChangeBorder = true;
            }
            if (isBottom)
            {
                thickness.Bottom = 2;
                isChangeBorder = true;
            }
            if (isRight)
            {
                thickness.Right = 2;
                isChangeBorder = true;
            }
            if (isLeft)
            {
                thickness.Left = 2;
                isChangeBorder = true;
            }
            if (isChangeBorder)
                cell.BorderBrush = Brushes.Crimson;

            cellStyle.Setters.Add(new Setter(BorderThicknessProperty, thickness));
            cell.Style = cellStyle;
        }

        public void FindingHeader()
        {
            DataGridRow dataGridRow = (DataGridRow)mainGridData.ItemContainerGenerator.ContainerFromItem(mainGridData.Items[0]);
            if (dataGridRow == null)
            {
                MessageBox.Show("No data. Can't find");
                return;
            }
            if (_crntFoundHeaderIdx != -1)
            {
                DataGridCell cell = DataGridServices.GetCell(mainGridData, 0, _crntFoundHeaderIdx);
                var cellStyle = new Style(typeof(DataGridCell));
                cellStyle.Setters.Add(new Setter(Control.BackgroundProperty, (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE599"))));
                cell.Style = cellStyle;
                _crntFoundHeaderIdx = -1;
            }
            foreach (var column in mainGridData.Columns)
            {
                DataGridCell cell = DataGridServices.GetCell(mainGridData, 0, column.DisplayIndex);
                if (cell == null)
                    continue;
                string cellData = (cell.Content as TextBlock).Text;
                if (cellData == HeaderSearchKey)
                {
                    var cellStyle = new Style(typeof(DataGridCell));
                    cellStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.GreenYellow));
                    cell.Style = cellStyle;
                    _crntFoundHeaderIdx = column.DisplayIndex;
                    break;
                }
            }
        }

    }
}
