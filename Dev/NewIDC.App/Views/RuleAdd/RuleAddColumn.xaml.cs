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

namespace NewIDC.App.Views.RuleAddColumn
{
    /// <summary>
    /// Interaction logic for RuleAddColumnScreen.xaml
    /// </summary>
    public partial class RuleAddColumnScreen : Window
    {
        private RuleAddColumnVM _vm;
        private DFileReferenceError fileReferenceError;
        private UserControl previosControl;
        public RuleAddColumnScreen(RuleAddColumnVM vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            RuleAddColumnBase ruleAddColumnBase = new RuleAddColumnBase(_vm);
            ruleAddColumnBase.SwitchControlRequested += ruleAddColumnBase_SwitchControlRequested;
            ContentPanel.Children.Add(ruleAddColumnBase);
        }

        private void ruleAddColumnBase_SwitchControlRequested(object sender, EventArgs e)
        {
            ContentPanel.Children.Clear();
            ContentPanel.Children.Add(new RuleAddEmptyColumn(_vm));
            ApplySettingDetail.Visibility = Visibility.Visible;
            _vm.buttonStep = RuleAddColumnVM.ButtonStep.RuleAddEmptyColumn;

        }

        private void NextStepBT_Click(object sender, RoutedEventArgs e)
        {
            UserControl nextStepControl = _vm.NextStep();
            if (nextStepControl != null)
            {
                ContentPanel.Children.Clear();

                ContentPanel.Children.Add(nextStepControl);
            }
        }

        private void PreviousStep_Click(object sender, RoutedEventArgs e)
        {
            UserControl previosStepControl = _vm.PreviousStep();
            if (previosStepControl != null)
            {
                ContentPanel.Children.Clear();
                var previousControl = previosStepControl as RuleAddColumnBase;
                if (previousControl != null)
                {
                    previousControl.SwitchControlRequested += ruleAddColumnBase_SwitchControlRequested;
                }
                ContentPanel.Children.Add(previosStepControl);
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
            UserControl nextStepControl = _vm.NextStep();
            if (nextStepControl != null)
            {
                ContentPanel.Children.Clear();

                ContentPanel.Children.Add(nextStepControl);
            }
        }
    }
}