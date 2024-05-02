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

namespace NewIDC.App.Views.RuleAddColumn
{
    /// <summary>
    /// Interaction logic for RuleAddColumnBase.xaml
    /// </summary>
    public partial class RuleAddColumnBase : UserControl
    {
        private bool isShrunk = false;
        private RuleAddColumnVM _vm;
        public event EventHandler SwitchControlRequested;
        public RuleAddColumnBase(RuleAddColumnVM vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
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
        }
        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (isShrunk)
            {
                ResizeButton.Content = ">>";
                ViewDatagrid.Width = 830;
                SettingControl.Width = 800;
                SettingControl.HorizontalAlignment = HorizontalAlignment.Left;
                isShrunk = false;
            }
            else
            {
                ResizeButton.Content = "<<";
                ViewDatagrid.Width = 1700;
                SettingControl.Width = 50;
                SettingControl.HorizontalAlignment = HorizontalAlignment.Right;
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

        private void AddEmptyColumn_Click(object sender, RoutedEventArgs e)
        {
            SwitchControlRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
