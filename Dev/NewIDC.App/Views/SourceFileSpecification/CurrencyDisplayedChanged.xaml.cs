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
    /// Interaction logic for CurrencyDisplayedChanged.xaml
    /// </summary>
    public partial class CurrencyDisplayedChanged : UserControl, IProjectSetting {
        private double originalWidth;
        private bool isShrunk = false;
        private readonly AmountFormatChangeSettingVM _vm;
        public CurrencyDisplayedChanged(AmountFormatChangeSettingVM vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            _vm.UpdateDatagrid += UpdateDataGrid;
            originalWidth = SettingControl.Width;
            Task.Factory.StartNew(() => { ImportData(); });
        }

        private void UpdateDataGrid(object sender, EventArgs e) {
            Dispatcher.BeginInvoke(new Action(() => {
                FileDataSetting.DataGridInput = _vm.CommonDataGridInput;
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        public void ImportData() {
            Dispatcher.BeginInvoke(new Action(() => {
                _vm.ImportDataToDatagrid();
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (isShrunk)
            {
                ResizeButton.Content = ">>";
                SettingControl.Width = originalWidth;
                SettingControl.HorizontalAlignment = HorizontalAlignment.Left;
                isShrunk = false;
            }
            else
            {
                ResizeButton.Content = "<<";
                SettingControl.Width = 50; 
                SettingControl.HorizontalAlignment = HorizontalAlignment.Right;
                isShrunk = true;
            }
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

        private void CheckHintFileSettingMoney_Checked(object sender, RoutedEventArgs e)
        {
            HintFileSettingMoney.IsEnabled = true;
        }

        private void CheckHintFileSettingMoney_Unchecked(object sender, RoutedEventArgs e)
        {
            HintFileSettingMoney.IsEnabled = false;
        }

        public void SaveSetting() {
            _vm.SaveSetting();   
            TopScreen topScreen = new TopScreen();
            topScreen.Show();
        }
    }
}
