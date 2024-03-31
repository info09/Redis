using System.Windows.Controls;
using WPF_Learning.App.ViewModel;

namespace WPF_Learning.App.UserControlApp
{
    /// <summary>
    /// Interaction logic for ControlBarUC.xaml
    /// </summary>
    public partial class ControlBarUC : UserControl
    {
        public ControlBarVM controlBarVM { get; set; }
        public ControlBarUC()
        {
            InitializeComponent();
            this.DataContext = controlBarVM = new ControlBarVM();
        }
    }
}
