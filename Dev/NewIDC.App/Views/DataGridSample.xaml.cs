using NewIDC.App.Models;
using NewIDC.App.Models.LibraryModels;
using NewIDC.App.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NewIDC.App.Views
{
    partial class SampleVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICollection _customDataGridSource { get; set; }
        public ICollection CustomDataGridSource
        {
            get => _customDataGridSource;
            set
            {
                _customDataGridSource = value;
                OnPropertyChanged(nameof(CustomDataGridSource));
            }
        }
        private ICollection _textErr_HeaderSource { get; set; }
        public ICollection TextErr_HeaderSource
        {
            get => _textErr_HeaderSource;
            set => _textErr_HeaderSource = value;
        }
        private ICollection _textErr_BodySource { get; set; }
        public ICollection TextErr_BodySource
        {
            get => _textErr_BodySource;
            set => _textErr_BodySource = value;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// Interaction logic for DataGridSample.xaml
    /// </summary>
    public partial class DataGridSample : Window
    {
        public DataGridSample()
        {
            InitializeComponent();
            var data = new List<RowData>
                {
                    new RowData
                    {
                        AKN = Faker.Company.Name(),
                        BKN = Faker.Company.Name(),
                        CKN = Faker.Company.Name(),
                        DKN = Faker.Company.Name(),
                    },
                    new RowData
                    {
                        AKN = Faker.Company.Name(),
                        BKN = Faker.Company.Name(),
                        CKN = Faker.Company.Name(),
                        DKN = Faker.Company.Name(),
                    },
                    new RowData
                    {
                        AKN = Faker.Company.Name(),
                        BKN = Faker.Company.Name(),
                        CKN = Faker.Company.Name(),
                        DKN = Faker.Company.Name(),
                    },
                    new RowData
                    {
                        AKN = Faker.Company.Name(),
                        BKN = Faker.Company.Name(),
                        CKN = Faker.Company.Name(),
                        DKN = Faker.Company.Name(),
                    }
                };
            CommonDataGridInput = data;
            customDatagrid.HeaderRowNumber = 3;
            DataContext = new SampleVM
            {
                CustomDataGridSource = data,
                TextErr_HeaderSource = data
            };
            //ComboboxHeaderItems = new HeaderInput();
            //ComboboxHeaderItems.HeaderType = HeaderInputType.Combobox;
            //var dataCombobox  = new List<ComboboxHeaderInput>();
            //int i = 0;
            //foreach (var item in data)
            //{
            //    i++;
            //    dataCombobox.Add(new ComboboxHeaderInput
            //    {
            //        ColumnIndex = i,
            //        Items = new List<ComboboxItem>
            //        {
            //            new ComboboxItem
            //            {
            //                Displayed = "ABC",
            //                Value = "1"
            //            },
            //            new ComboboxItem
            //            {
            //                Displayed = "XYZ",
            //                Value = "2"
            //            }
            //        },
            //        SelectedValue = "1",
            //        TextBelow = ""
            //    });
            //}
            //ComboboxHeaderItems.Data = dataCombobox;
            ComboboxHeaderItems = new HeaderInput
            {
                HeaderType = HeaderInputType.Combobox,
                Data = new List<ComboboxHeaderInput>(){
                new ComboboxHeaderInput
                {
                    ColumnIndex = 1,
                    Items = new List<ComboboxItem>
                    {
                        new ComboboxItem
                        {
                            Displayed = "ABC",
                            Value = "1"
                        },
                        new ComboboxItem
                        {
                            Displayed = "XYZ",
                            Value = "2"
                        }
                    },
                    SelectedValue = "1",
                    TextBelow = ""
                },
                    //new CheckboxHeaderInput
                    //{
                    //    CheckboxText = "Test",
                    //    ColumnIndex = 2,
                    //    HeaderType = HeaderInputType.Checkbox,
                    //    IsChecked = false
                    //}
                    //new TextErrorHeaderInput
                    //{
                    //    ColumnIndex = 1,
                    //    HeaderType = HeaderInputType.Textbox,
                    //    Text = "Error 1",
                    //    IconPath = "F:\\Projects\\Gitlab\\new_idc_phase_1\\NewIDC.App\\Icons\\warning.png"
                    //}
                }
            };
            coreDataGrid.HeaderItems = ComboboxHeaderItems;
            coreDataGrid.DataGridItems = data;
            GridData = data;
        }
        public HeaderInput ComboboxHeaderItems
        {
            get => _comboboxHeaderItems;
            set => _comboboxHeaderItems = value;
        }
        private HeaderInput _comboboxHeaderItems { get; set; }

        public List<RowData> GridData
        {
            get => _gridData;
            set => _gridData = value;
        }
        private List<RowData> _gridData { get; set; }

        private void OnCommonDataGridInput_Changed()
        {
            customDatagrid.DataGridInput = _commonDataGridInput;
        }
        private ICollection _commonDataGridInput
        { get; set; }
        public ICollection CommonDataGridInput
        {
            get => _commonDataGridInput;
            set
            {
                _commonDataGridInput = value;
                OnCommonDataGridInput_Changed();
            }
        }
        public double ZoomScale
        {
            get => _zoomScale;
            set => _zoomScale = value;
        }
        private double _zoomScale { get; set; }
        public class RowData
        {
            public string AKN { get; set; }
            public string BKN { get; set; }
            public string CKN { get; set; }
            public string DKN { get; set; }
        }


        private void StartColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            customDatagrid.ColumnIdxSelectedBegin = StartColumn.Text;
        }

        private void ColumnNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            customDatagrid.ColumnSelectedNumber = ColumnNumber.Text;
        }

        private void StartRow_TextChanged(object sender, TextChangedEventArgs e)
        {
            customDatagrid.RowIdxSelectedBegin = StartRow.Text;
        }

        private void RowNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            customDatagrid.RowSelectedNumber = RowNumber.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            customDatagrid.ZoomScale += 0.1;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            customDatagrid.ZoomScale -= 0.1;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            customDatagrid.HeaderSearchKey = SearchBox.Text;
        }

        private void ColoringRow_Click(object sender, RoutedEventArgs e)
        {
            customDatagrid.ColoringRow = new List<ColoringRow>
            {
                new ColoringRow
                {
                    ColorBrush = Brushes.Aqua,
                    RowIndex = 1,
                }
            };
        }

        private void ColoringCol_Click(object sender, RoutedEventArgs e)
        {
            customDatagrid.ColoringColumn = new List<ColoringColumn>
            {
                new ColoringColumn
                {
                    ColorBrush = Brushes.Coral,
                    ColumnIndex = 1,
                }
            };
        }
    }
}
