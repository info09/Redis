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
    /// Interaction logic for NewTitleFile.xaml
    /// </summary>
    public partial class NewTitleFile : UserControl
    {
        private readonly SourceFileVM _vm;
        public NewTitleFile(SourceFileVM vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        
    }
}
