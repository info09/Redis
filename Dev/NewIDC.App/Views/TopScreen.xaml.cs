
using NewIDC.App.ViewModels;
using NewIDC.App.Views.RuleAddColumn;
using NewIDC.App.Views.SourceFileSpecificayion;
using NewIDC.Projects;
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
using System.Windows.Shapes;

namespace NewIDC.App.Views
{
    /// <summary>
    /// Interaction logic for TopScreen.xaml
    /// </summary>
    public partial class TopScreen : Window
    {
        public TopScreen()
        {
            InitializeComponent();
        }

        private void SettingButon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InstructButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ConfigButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SourceFileSpecificationScreen sourceFileSpecificationScreen = new SourceFileSpecificationScreen(new ViewModels.SourceFileVM());
            this.Close();
            sourceFileSpecificationScreen.Show();
            //RuleAddColumnScreen ruleAddColumnScreen = new RuleAddColumnScreen(new RuleAddColumnVM());
            //this.Close();
            //ruleAddColumnScreen.Show();

        }

        private void EditProjectButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TestExecutionProjectScreen projectScreen = new TestExecutionProjectScreen(new IniProjectRepository());
            projectScreen.Show();
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
