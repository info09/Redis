using NewIDC.App.ViewModels;
using NewIDC.App.Views.SourceFileSpecification;
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

namespace NewIDC.App.Views.SourceFileSpecificayion
{
    /// <summary>
    /// Interaction logic for SourceFileSpecificationScreen.xaml
    /// </summary>
    public partial class SourceFileSpecificationScreen : Window
    {
        private readonly SourceFileVM _vm;
        private DFileReferenceError fileReferenceError;
        private IProjectSetting previosControl;
        public SourceFileSpecificationScreen(SourceFileVM vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            NewTitleFile newTitleFile = new NewTitleFile(_vm);
            //specifyFile = new SpecifyFile(_vm);
            ContentPanel.Children.Add(newTitleFile);
        }

        private void NextStepBT_Click(object sender, RoutedEventArgs e)
        {
            IProjectSetting nextStepControl = _vm.NextStep();
            if (nextStepControl != null)
            {
                ContentPanel.Children.Clear();

                ContentPanel.Children.Add(nextStepControl as UserControl);
                fileReferenceError = nextStepControl as DFileReferenceError;
                if (fileReferenceError != null)
                {
                    fileReferenceError.OKButtonClicked += FileReferenceError_OKButtonClicked;
                } else
                {
                    previosControl = nextStepControl;
                }
            }
        }

        private void FileReferenceError_OKButtonClicked(object sender, EventArgs e)
        {
            ContentPanel.Children.Remove(fileReferenceError);
            ContentPanel.Children.Add(previosControl as UserControl);
        }

        private void PreviousStep_Click(object sender, RoutedEventArgs e)
        {
            UserControl previosStepControl = _vm.PreviousStep();
            if (previosStepControl != null)
            {
                ContentPanel.Children.Clear();
                ContentPanel.Children.Add(previosStepControl);
                previosControl = null;
            }
           
        }

        private void Setting_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SystemSettingScreen systemSettingScreen = new SystemSettingScreen();
            systemSettingScreen.Show();
        }

        private void EditProjectNameButton_Click(object sender, RoutedEventArgs e)
        {
           BackGroundDock.Visibility = Visibility.Visible;
        }

        private void ExitDEditButton_Click(object sender, RoutedEventArgs e)
        {
            BackGroundDock.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool checkFileName = ((SourceFileVM)this.DataContext).CheckInput_PrjName(EditProjectName.Text);
            if (checkFileName)
            {
                ((SourceFileVM)this.DataContext).ProjectNameInput = EditProjectName.Text;
                BackGroundDock.Visibility = Visibility.Collapsed;
            }
            
        }

        private void ExitNameProject_Click(object sender, RoutedEventArgs e)
        {
            DCancel.Visibility = Visibility.Visible;
        }

        private void CancelDCancel_Click(object sender, RoutedEventArgs e)
        {
            DCancel.Visibility = Visibility.Collapsed;
        }

        private void ApplyDCancel_Click(object sender, RoutedEventArgs e)
        {
            TopScreen topScreen = new TopScreen();
            this.Close();
            topScreen.Show();
        }

        private void ApplySettingDetail_Click(object sender, RoutedEventArgs e)
        {
            if (previosControl != null) {
                previosControl.SaveSetting();
            }
            IProjectSetting nextStepControl = _vm.NextStep();
            if (nextStepControl != null)
            {
                ContentPanel.Children.Clear();

                ContentPanel.Children.Add(nextStepControl as UserControl);
                previosControl = nextStepControl;
            } else {
                this.Close();
            }
        }
    }
}
