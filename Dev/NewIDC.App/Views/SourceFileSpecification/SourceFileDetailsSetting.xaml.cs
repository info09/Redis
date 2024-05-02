using NewIDC.App.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewIDC.App.Views.SourceFileSpecification
{
    /// <summary>
    /// Interaction logic for SourceFileDetailsSetting.xaml
    /// </summary>
    public partial class SourceFileDetailsSetting : UserControl, IProjectSetting {
        private double originalWidth;
        private bool isShrunk = false;
        private readonly SourceFileVM _vm;
        public SourceFileDetailsSetting(SourceFileVM vm)
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
            _vm = vm;
            DataContext = _vm;
            originalWidth = SettingControl.Width;
        }

        private void CheckHintTextBox_Checked(object sender, RoutedEventArgs e)
        {
            TextboxHintProject.IsEnabled = true;
        }

        private void CheckHintTextBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TextboxHintProject.IsEnabled = false;
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (isShrunk)
            {
                ResizeButton.Content = ">>";
                SearchHeaderBoder.HorizontalAlignment = HorizontalAlignment.Center;
                ViewDatagrid.Width = 1120;
                SettingControl.Width = originalWidth;
                SettingControl.HorizontalAlignment = HorizontalAlignment.Left;
                isShrunk = false;
            }
            else
            {
                ResizeButton.Content = "<<";
                SearchHeaderBoder.HorizontalAlignment = HorizontalAlignment.Right;
                ViewDatagrid.Width = 1700;
                SettingControl.Width = 50; 
                isShrunk = true;
            }
        }
        private void OnCommonDataGridInput_Changed()
        {
            FileDataSetting.DataGridInput = _commonDataGridInput;
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
        private class RowData
        {
            public string AKN { get; set; }
            public string BKN { get; set; }
            public string CKN { get; set; }
            public string DKN { get; set; }
        }

        private void ZoomOutDG_Click(object sender, RoutedEventArgs e)
        {
            FileDataSetting.ZoomScale -= 0.1;
            NumberScale.Text = (FileDataSetting.ZoomScale * 100).ToString() + "%";
        }

        private void ZoomInDG_Click(object sender, RoutedEventArgs e)
        {
            FileDataSetting.ZoomScale += 0.1;
            NumberScale.Text = (FileDataSetting.ZoomScale * 100).ToString() + "%";
        }

        private void OpenSearchHedaer_Click(object sender, RoutedEventArgs e)
        {
            SearchHeader.Visibility = Visibility.Visible;
            OpenSearchHedaer.IsEnabled = false;
        }

        private void CloseSearchHeader_Click(object sender, RoutedEventArgs e)
        {
            SearchHeader.Visibility = Visibility.Collapsed;
            OpenSearchHedaer.IsEnabled = true;
        }

        private void SearchHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            FileDataSetting.HeaderSearchKey = SearchBox.Text;
        }

        public void SaveSetting() {
        }
    }
}
