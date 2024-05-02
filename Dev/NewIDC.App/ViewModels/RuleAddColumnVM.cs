using NewIDC.App.Views.SourceFileSpecification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NewIDC.App.ViewModels.SourceFileVM;
using System.Windows.Controls;
using System.Windows;
using NewIDC.App.Views.RuleAddColumn;

namespace NewIDC.App.ViewModels
{
    public class RuleAddColumnVM : INotifyPropertyChanged
    {
        public ButtonStep buttonStep { get; set; }

        public RuleAddColumnVM()
        {
            NextStepVisibility = Visibility.Collapsed;
            ConversionEditVisibility = Visibility.Collapsed;

            NameRuleConvert = "変換対象：0. 変換元テーブル";

            ItemsLine = new List<string>
            {
                "10行",
                "50行",
                "100行",
                "500行",
                "1000行"

            };
            SelectedLine = "10行";
        }
        private Visibility _nextStepVisibility;
        public Visibility NextStepVisibility
        {
            get { return _nextStepVisibility; }
            set
            {
                if(_nextStepVisibility != value)
                {
                    _nextStepVisibility = value;
                    OnPropertyChanged(nameof(NextStepVisibility));
                }
            }
        }
        private Visibility _conversionEditVisibility;
        public Visibility ConversionEditVisibility
        {
            get { return _conversionEditVisibility; }
            set
            {
                if (_conversionEditVisibility != value)
                {
                    _conversionEditVisibility = value;
                    OnPropertyChanged(nameof(ConversionEditVisibility));
                }
            }
        }
        private string _nameRuleConvert;
        public string NameRuleConvert
        {
            get { return _nameRuleConvert; }
            set
            {
                _nameRuleConvert = value;
                OnPropertyChanged(nameof(NameRuleConvert));
            }
        }


        public List<string> ItemsLine { get; set; }
        private string _selectedLine;
        public string SelectedLine
        {
            get { return _selectedLine; }
            set
            {
                if (_selectedLine != value)
                {
                    _selectedLine = value;
                    OnPropertyChanged(nameof(SelectedLine));

                }
            }
        }



        public enum ButtonStep
        {
           RuleAddColumnBase = 0,
           RuleAddEmptyColumn = 1,
        }
        public UserControl NextStep()
        {
            switch (buttonStep)
            {
                case ButtonStep.RuleAddColumnBase:
                    return null;

                default:
                    return null;

            }
        }
        public UserControl PreviousStep()
        {
            switch(buttonStep)
            {
                case ButtonStep.RuleAddColumnBase:
                    ConversionEditVisibility = Visibility.Collapsed;
                    return null;
                case ButtonStep.RuleAddEmptyColumn:
                    ConversionEditVisibility = Visibility.Collapsed;
                    RuleAddColumnBase ruleAddColumnBase = new RuleAddColumnBase(this);
                    buttonStep = ButtonStep.RuleAddColumnBase;
                    return ruleAddColumnBase;



                default:
                    return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
