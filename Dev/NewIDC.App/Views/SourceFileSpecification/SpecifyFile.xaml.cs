using NewIDC.App.ViewModels;
using System;
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
    /// Interaction logic for SpecifyFile.xaml
    /// </summary>
    public partial class SpecifyFile : UserControl, IProjectSetting {
        private readonly SourceFileVM _vm;
        public SpecifyFile(SourceFileVM vm)
        {
            InitializeComponent();
            RDONumberSheet.IsChecked = true;
            CBBSheetNumber.IsEnabled = true;
            CBBSheetName.IsEnabled = false;
            _vm = vm;
            DataContext = _vm;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            TxTSelectFile.Visibility = Visibility.Visible;
            TxTWildcard.Visibility = Visibility.Visible;
            CBBWildcard.Visibility = Visibility.Visible;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            TxTSelectFile.Visibility = Visibility.Hidden;
            TxTWildcard.Visibility = Visibility.Hidden;
            CBBWildcard.Visibility = Visibility.Hidden;
        }

        private void RdoOption_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selectedRadioButton = (RadioButton)sender;

            if (selectedRadioButton.IsChecked == true)
            {
                if (selectedRadioButton == RDONumberSheet)
                {
                    // Kích hoạt CBBSheetNumber và vô hiệu hóa RDOSheetName
                    CBBSheetNumber.IsEnabled = true;
                    RDOSheetName.IsChecked = false;
                    CBBSheetName.IsEnabled = false;
                }
                else if (selectedRadioButton == RDOSheetName)
                {
                    // Kích hoạt CBBSheetName và vô hiệu hóa RDONumberSheet
                    RDONumberSheet.IsChecked = false;
                    CBBSheetNumber.IsEnabled = false;
                    CBBSheetName.IsEnabled = true;
                }
            }
        }

        public void SaveSetting() {
            throw new NotImplementedException();
        }
    }
}
