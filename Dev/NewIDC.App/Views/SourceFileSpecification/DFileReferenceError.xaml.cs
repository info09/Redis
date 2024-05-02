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
    /// Interaction logic for DFileReferenceError.xaml
    /// </summary>
    public partial class DFileReferenceError : UserControl, IProjectSetting {
        public event EventHandler OKButtonClicked;
        private SourceFileVM _vm;
        public DFileReferenceError(SourceFileVM vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

       

        private void ButtonErrorFile_Click_1(object sender, RoutedEventArgs e)
        {
            OKButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        public void SaveSetting() {
            throw new NotImplementedException();
        }
    }
}
