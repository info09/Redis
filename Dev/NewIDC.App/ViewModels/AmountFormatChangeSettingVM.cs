using NewIDC.Projects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NewIDC.App.ViewModels {
    public class AmountFormatChangeSettingVM : BaseVM {
        public EventHandler UpdateDatagrid;
        private string nameFileConvert;
        public string NameFileConvert {
            get { return nameFileConvert; }
            set {
                if (nameFileConvert != value) {
                    nameFileConvert = value;
                    OnPropertyChanged(nameof(NameFileConvert));
                }
            }
        }
        public List<string> ItemsLine { get; set; }
        private string _selectedLine;
        public string SelectedLine {
            get { return _selectedLine; }
            set {
                if (_selectedLine != value) {
                    _selectedLine = value;
                    OnPropertyChanged(nameof(SelectedLine));

                }
            }
        }
        public List<string> ItemsThousandsSeparator { get; set; }
        private string _selectedThousandsSeparator;
        public string SelectedThousandsSeparator {
            get { return _selectedThousandsSeparator; }
            set {
                if (_selectedThousandsSeparator != value) {
                    _selectedThousandsSeparator = value;
                    OnPropertyChanged(nameof(SelectedThousandsSeparator));

                }
            }
        }
        public List<string> ItemsFirstMoney { get; set; }
        private string _selectedFirstMoney;
        public string SelectedFirstMoney {
            get { return _selectedFirstMoney; }
            set {
                if (_selectedFirstMoney != value) {
                    _selectedFirstMoney = value;
                    OnPropertyChanged(nameof(SelectedFirstMoney));
                    VisibilityFirstMoney = (SelectedFirstMoney == "何もしない") ? Visibility.Collapsed : Visibility.Visible;

                }
            }
        }
        public List<string> ItemsLastMoney { get; set; }
        private string _selectedLastMoney;
        public string SelectedLastMoney {
            get { return _selectedLastMoney; }
            set {
                if (_selectedLastMoney != value) {
                    _selectedLastMoney = value;
                    OnPropertyChanged(nameof(SelectedLastMoney));
                    VisibilityLastMoney = (SelectedLastMoney == "何もしない") ? Visibility.Collapsed : Visibility.Visible;

                }
            }
        }
        private Visibility _visibilityLastMoney;
        public Visibility VisibilityLastMoney {
            get { return _visibilityLastMoney; }
            set {
                if (_visibilityLastMoney != value) {
                    _visibilityLastMoney = value;
                    OnPropertyChanged(nameof(VisibilityLastMoney));
                }
            }
        }
        private Visibility _visibilityFirstMoney;
        public Visibility VisibilityFirstMoney {
            get { return _visibilityFirstMoney; }
            set {
                if (_visibilityFirstMoney != value) {
                    _visibilityFirstMoney = value;
                    OnPropertyChanged(nameof(VisibilityFirstMoney));
                }
            }
        }
        private Visibility _visibilityAppend;
        public Visibility VisibilityAppend {
            get { return _visibilityAppend; }
            set {
                if (_visibilityAppend != value) {
                    _visibilityAppend = value;
                    OnPropertyChanged(nameof(VisibilityAppend));
                }
            }
        }
        private int _valueAppend;
        public int ValueAppend {
            get { return _valueAppend; }
            set {
                _valueAppend = value;
                OnPropertyChanged(nameof(ValueAppend));
            }
        }
        public List<string> ItemsAppend { get; set; }

        private string _selectedAppend;
        public string SelectedAppend {
            get { return _selectedAppend; }
            set {
                if (_selectedAppend != value) {
                    _selectedAppend = value;
                    OnPropertyChanged(nameof(SelectedAppend));
                    VisibilityAppend = (SelectedAppend == "付加する") ? Visibility.Visible : Visibility.Collapsed;

                }
            }
        }
        public string PreviousSymbol { get; set; }
        public string AfterSymbol { get; set; }
        public int TargetColumnIndex { get; set; }
        private ICollection _commonDataGridInput { get; set; }
        public ICollection CommonDataGridInput {
            get => _commonDataGridInput;
            set {
                _commonDataGridInput = value;
                OnPropertyChanged(nameof(CommonDataGridInput));
            }
        }
        private string sourceFilePath;
        private ProjectConfigController projectConfigController = ProjectConfigController.GetInstance();
        private bool isSourceFilePathChanged = false;
        public AmountFormatChangeSettingVM()
        {
            ItemsLine = new List<string>
            {
                "10行",
                "50行",
                "100行",
                "500行",
                "1000行"
            };
            SelectedLine = "10行";
            ItemsFirstMoney = new List<string>
            {
                "何もしない",
                "追加する",
                "削除する",
            };
            SelectedFirstMoney = "何もしない";
            ItemsLastMoney = new List<string>
            {
                "何もしない",
                "追加する",
                "削除する",
            };
            SelectedLastMoney = "何もしない";
            ItemsThousandsSeparator = new List<string>
            {
                "カンマを付加する",
                "カンマを付加しない",
            };
            SelectedThousandsSeparator = "カンマを付加する";
            ItemsAppend = new List<string>
            {
                "付加する",
                "付加しない",
            };
            SelectedAppend = "付加する";
            ValueAppend = 1;
            TargetColumnIndex = 2;//dummy
            projectConfigController.PropertiesChanged += OnProjectConfigChanged;
        }

        private void OnProjectConfigChanged(object sender, EventArgs e) {
            Task taskA = new Task(() => ImportDataToDatagrid());
            taskA.Start();
        }

        public void SaveSetting() {
            string[] param = new string[10];
            param[0] = "Money";
            param[1] = "1";
            param[2] = "T" + (TargetColumnIndex + 1);
            param[3] = ItemsFirstMoney.FindIndex(s => s.Equals(SelectedFirstMoney)).ToString();
            param[4] = PreviousSymbol;
            param[5] = ItemsLastMoney.FindIndex(s=>s.Equals(SelectedLastMoney)).ToString();
            param[6] = AfterSymbol;
            param[7] = (ItemsThousandsSeparator.FindIndex(s=>s.Equals(SelectedThousandsSeparator)) + 1).ToString();
            param[8] = (ItemsAppend.FindIndex(s=>s.Equals(SelectedAppend)) + 1).ToString();
            param[9] = ValueAppend.ToString();
            ConversionBase conv = new MoneyConversion(param, TargetColumnIndex);
            projectConfigController.AddConversion(conv);
            projectConfigController.SaveConfig();
        }
        private ICollection GetSourceFileContent() {
            List<string[]> content = new List<string[]>();
            ProjectConfig projectConfig = projectConfigController.GetProjectConfig();
            if (string.IsNullOrEmpty(projectConfig.SourceFilePath)) {
                return null;
            }
            if (CommonDataGridInput != null && projectConfig.SourceFilePath == sourceFilePath) {
                isSourceFilePathChanged = false;
                return CommonDataGridInput;
            }
            sourceFilePath = projectConfig.SourceFilePath;
            isSourceFilePathChanged = true;
            FirstConversion conv = new FirstConversion(projectConfig.SourceFilePath);
            var rows = conv.Convert();
            content.Add(conv.GetHeader());
            content.AddRange(rows);
            return content;
        }
        public void ImportDataToDatagrid() {
            CommonDataGridInput = GetSourceFileContent();
            if (CommonDataGridInput != null && isSourceFilePathChanged) {
                UpdateDatagrid?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
