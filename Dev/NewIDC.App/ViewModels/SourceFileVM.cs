using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using NewIDC.App.Views.SourceFileSpecification;
using NewIDC.App.ViewModels.Command;
using System.IO;
using NewIDC.Projects;
using NewIDC.App.Views;

namespace NewIDC.App.ViewModels
{
    public class SourceFileVM : INotifyPropertyChanged
    {
        public SourceFileVM()
        {
            ErrorLabelVisible = Visibility.Collapsed;
            ButtonVisibility = Visibility.Hidden;
            ExcelSetting = Visibility.Collapsed;
            ProjectTitle = Visibility.Collapsed;
            DownloadSpecify = Visibility.Collapsed;
            CheckFileFormatLabel = Visibility.Collapsed;
            SheetNameSetting = Visibility.Collapsed;
            SettingSourceExcel = Visibility.Collapsed;
            ConversionEdit = Visibility.Collapsed;
            NextStepVisibility = Visibility.Visible;

            ValueSheetNumber = 1;
            ValueAppend = 1;
            HeaderLine = 1;
            ProjectNoteEnabled = false;

            IsChecked = false;
            OpenFileDialog = new RelayCommand(OpenFileDlg);
            FileFormatItems = new List<string>
            {
                "CSV",
                "TSV",
                "Excel"
            };
            SelectedFileFormat = "CSV";
            ItemsCharactercode = new List<string>
            {
                "UTF-8",
                "Shift-JIS"
            };
            SelectedCharactercode = "UTF-8";
            ItemsWildCard = new List<string>
            {
                "Time",
                "Name"
            };
            SelectedWildCard = "Time";
            ItemsHeaderUse = new List<string>
            {
                "あり",
                "なし"
            };
            SelectedHeaderUse = "あり";
            ItemsSheetNameUse = new List<string>
            {
                "Sheet1",
                "Sheet2",
                "Sheet3"
            };
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
            this.projectConfigController = ProjectConfigController.GetInstance();
            this.projectConfigController.AddNewProjectConfig();
        }
        private ProjectConfigController projectConfigController;
        private string nameFileConvert;
        public string NameFileConvert
        {
            get { return nameFileConvert; }
            set
            {
                if(nameFileConvert != value)
                {
                    nameFileConvert = value;
                    OnPropertyChanged(nameof(NameFileConvert));
                }
            }
        }
        private string filePathName;
        public string FilePathName
        {
            get { return filePathName; }
            set
            {
                if (filePathName != value)
                {
                    filePathName = value;
                    OnPropertyChanged(nameof(FilePathName));
                }
            }
        }
        private string editProjectName;
        public string EditProjectName
        {
            get { return editProjectName; }
            set
            {
                if (editProjectName != value)
                {
                    editProjectName = value;
                    OnPropertyChanged(nameof(EditProjectName));

                }
            }
        }
        private string changeProjectName;
        public string ChangeProjectName
        {
            get { return changeProjectName; }
            set
            {
                if (changeProjectName != value)
                {
                    changeProjectName = value;
                    OnPropertyChanged(nameof(ChangeProjectName));

                }
            }
        }
        private string _projectName;
        public string ProjectNameInput
        {
            get { return _projectName; }
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnPropertyChanged(nameof(ProjectNameInput));
                    this.EditProjectName = "編集中：" + this.ProjectNameInput;
                    ChangeProjectName = ProjectNameInput;
                    FillPropertiesIntoProjectConfig();
                }
            }
        }
        private int _valueSheetNumber;
        public int ValueSheetNumber
        {
            get { return _valueSheetNumber; }
            set
            {
                _valueSheetNumber = value;
                OnPropertyChanged(nameof(ValueSheetNumber));
            }
        }
        private int _valueAppend;
        public int ValueAppend
        {
            get { return _valueAppend; }
            set
            {
                _valueAppend = value;
                OnPropertyChanged(nameof(ValueAppend));
            }
        }
        private int _headerLine;
        public int HeaderLine
        {
            get { return _headerLine; }
            set
            {
                _headerLine = value;
                OnPropertyChanged(nameof(HeaderLine));
            }
        }
        private string projectNote;
        public string ProjectNote
        {
            get { return projectNote; }
            set
            {
                if (projectNote != value)
                {
                    projectNote = value;
                    OnPropertyChanged(nameof(ProjectNote));
                }
            }
        }
        private string sourceFile;
        public string SourceFileSpecification
        {
            get { return sourceFile; }
            set
            {
                if (sourceFile != value)
                {
                    sourceFile = value;
                    OnPropertyChanged(nameof(SourceFileSpecification));
                    FillPropertiesIntoProjectConfig();
                }
            }
        }
        private string errorMessageName;
        public string ErrorMessageLabelName
        {
            get { return errorMessageName; }
            set
            {
                if (errorMessageName != value)
                {
                    errorMessageName = value;
                    OnPropertyChanged(nameof(ErrorMessageLabelName));
                }
            }
        }
        private string errorMessageSoucre;
        public string ErrorMessageLabelSoucre
        {
            get { return errorMessageSoucre; }
            set
            {
                if (errorMessageSoucre != value)
                {
                    errorMessageSoucre = value;
                    OnPropertyChanged(nameof(ErrorMessageLabelSoucre));
                }
            }
        }

        private bool _isRDONumberSheetChecked;
        public bool IsRDONameSheetChecked
        {
            get { return _isRDONumberSheetChecked; }
            set
            {
                if (_isRDONumberSheetChecked != value)
                {
                    _isRDONumberSheetChecked = value;
                    OnPropertyChanged(nameof(IsRDONameSheetChecked));
                }
            }
        }


        private Visibility _errorLabel;
        public Visibility ErrorLabelVisible
        {
            get { return _errorLabel; }
            set
            {
                if (_errorLabel != value)
                {
                    _errorLabel = value;
                    OnPropertyChanged(nameof(ErrorLabelVisible));
                }
            }
        }
        private Visibility _visibilityFirstMoney;
        public Visibility VisibilityFirstMoney
        {
            get { return _visibilityFirstMoney; }
            set
            {
                if (_visibilityFirstMoney != value)
                {
                    _visibilityFirstMoney = value;
                    OnPropertyChanged(nameof(VisibilityFirstMoney));
                }
            }
        }
        private Visibility _visibilityAppend;
        public Visibility VisibilityAppend
        {
            get { return _visibilityAppend; }
            set
            {
                if (_visibilityAppend != value)
                {
                    _visibilityAppend = value;
                    OnPropertyChanged(nameof(VisibilityAppend));
                }
            }
        }
        private Visibility _visibilityLastMoney;
        public Visibility VisibilityLastMoney
        {
            get { return _visibilityLastMoney; }
            set
            {
                if (_visibilityLastMoney != value)
                {
                    _visibilityLastMoney = value;
                    OnPropertyChanged(nameof(VisibilityLastMoney));
                }
            }
        }
        private Visibility _conversionEdit;
        public Visibility ConversionEdit
        {
            get { return _conversionEdit; }
            set
            {
                if (_conversionEdit != value)
                {
                    _conversionEdit = value;
                    OnPropertyChanged(nameof(ConversionEdit));
                }
            }
        }
        private Visibility _nextStepVisibility;
        public Visibility NextStepVisibility
        {
            get { return _nextStepVisibility; }
            set
            {
                if (_nextStepVisibility != value)
                {
                    _nextStepVisibility = value;
                    OnPropertyChanged(nameof(NextStepVisibility));
                }
            }
        }
        private Visibility _onHeaderLine;
        public Visibility OnHeaderLine
        {
            get { return _onHeaderLine; }
            set
            {
                if (_onHeaderLine != value)
                {
                    _onHeaderLine = value;
                    OnPropertyChanged(nameof(OnHeaderLine));
                }
            }
        }
        private Visibility _settingSourceExcel;
        public Visibility SettingSourceExcel
        {
            get { return _settingSourceExcel; }
            set
            {
                if (_settingSourceExcel != value)
                {
                    _settingSourceExcel = value;
                    OnPropertyChanged(nameof(SettingSourceExcel));
                }
            }
        }
        private Visibility _isButtonVisible;
        public Visibility ButtonVisibility
        {
            get { return _isButtonVisible; }
            set
            {
                if (_isButtonVisible != value)
                {
                    _isButtonVisible = value;
                    OnPropertyChanged(nameof(ButtonVisibility));
                }
            }
        }
        private Visibility _isProjectTitlee;
        public Visibility ProjectTitle
        {
            get { return _isProjectTitlee; }
            set
            {
                if (_isProjectTitlee != value)
                {
                    _isProjectTitlee = value;
                    OnPropertyChanged(nameof(ProjectTitle));
                }
            }
        }
        private Visibility _isCheckFileFormatLabel;
        public Visibility CheckFileFormatLabel
        {
            get { return _isCheckFileFormatLabel; }
            set
            {
                if (_isCheckFileFormatLabel != value)
                {
                    _isCheckFileFormatLabel = value;
                    OnPropertyChanged(nameof(CheckFileFormatLabel));
                }
            }
        }
        private Visibility _isDownloadSpecify;
        public Visibility DownloadSpecify
        {
            get { return _isDownloadSpecify; }
            set
            {
                if (_isDownloadSpecify != value)
                {
                    _isDownloadSpecify = value;
                    OnPropertyChanged(nameof(DownloadSpecify));
                }
            }
        }
        private Visibility _isExcelSetting;
        public Visibility ExcelSetting
        {
            get { return _isExcelSetting; }
            set
            {
                if (_isExcelSetting != value)
                {
                    _isExcelSetting = value;
                    OnPropertyChanged(nameof(ExcelSetting));

                }
            }
        }
        private Visibility _isSheetNameSetting;
        public Visibility SheetNameSetting
        {
            get { return _isSheetNameSetting; }
            set
            {
                if (_isSheetNameSetting != value)
                {
                    _isSheetNameSetting = value;
                    OnPropertyChanged(nameof(SheetNameSetting));

                }
            }
        }
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(_isChecked));
                    if (_isChecked)
                    {
                        ProjectNoteEnabled = true;
                        ProjectNoteForeground = Brushes.Black;
                        return;
                    }
                    ProjectNoteEnabled = false;
                    ProjectNoteForeground = Brushes.Gray;
                }
            }
        }
        private bool _projectNoteEnabled;
        public bool ProjectNoteEnabled
        {
            get { return _projectNoteEnabled; }
            set
            {
                if (_projectNoteEnabled != value)
                {
                    _projectNoteEnabled = value;
                    OnPropertyChanged(nameof(ProjectNoteEnabled));
                }
            }
        }
        private Brush _projectNoteForeground;
        public Brush ProjectNoteForeground
        {
            get { return _projectNoteForeground; }
            set
            {
                if (_projectNoteForeground != value)
                {
                    _projectNoteForeground = value;
                    OnPropertyChanged(nameof(ProjectNoteForeground));
                }
            }
        }
        private string _introLabel;
        public string IntroLabel
        {
            get { return _introLabel; }
            set
            {
                if (_introLabel != value)
                {
                    _introLabel = value;
                    OnPropertyChanged(nameof(IntroLabel));
                }
            }
        }
        public ICommand OpenFileDialog { get; }
        private void OpenFileDlg()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SourceFileSpecification = openFileDialog.FileName;
                FilePathName = Path.GetFileName(openFileDialog.FileName);
            }
        }
        private ButtonStep buttonStep { get; set; }
        public enum ButtonStep
        {
            NewTitleFile = 0,
            SpecifyFile = 1,
            DFileReferenceError = 2,
            SourceFileDetailsSetting = 3,
            CurrencyDisplayedChange = 4,
        }
        private Dictionary<ButtonStep, UserControl> nextUserControlTable = new Dictionary<ButtonStep, UserControl>() {};

        public bool CheckInput_PrjName(string input)
        {

            char[] invalidSymbols = { '!', '?', '@', '#', '[', ']', '(', ')', '-', '=' };
            if (string.IsNullOrEmpty(input))
            {
                ErrorMessageLabelName = "プロジェクト名を入力してください。";
                ErrorLabelVisible = Visibility.Visible;
                return false;
            }
            if (invalidSymbols.Any(symbol => input.Contains(symbol)))
            {
                ErrorMessageLabelName = "プロジェクト名に無効な文字が含まれています。";
                ErrorLabelVisible = Visibility.Visible;
                return false;
            }
            ErrorLabelVisible = Visibility.Hidden;
            return true;
        }
        public bool CheckInput_SrcFile(string input)
        {
            char[] invalidSymbols = { '!', '?', '@', '#', '[', ']', '(', ')', '-', '=' };
            if (string.IsNullOrEmpty(input))
            {
                ErrorMessageLabelSoucre = "変換するソースファイルを指定してください。";
                ErrorLabelVisible = Visibility.Visible;
                return false;
            }
            if (invalidSymbols.Any(symbol => input.Contains(symbol)))
            {
                ErrorMessageLabelSoucre = "変換元ファイル名に無効な文字が含まれています。";
                ErrorLabelVisible = Visibility.Visible;
                return false;
            }
            return true;
        }
        public List<string> FileFormatItems { get; set; }
        private string _selectedFileFormat;
        public string SelectedFileFormat
        {
            get { return _selectedFileFormat; }
            set
            {
                if (_selectedFileFormat != value)
                {
                    _selectedFileFormat = value;
                    OnPropertyChanged(nameof(SelectedFileFormat));
                    ExcelSetting = _selectedFileFormat == "Excel" ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public List<string> ItemsSheetNameUse { get; set; }
        private string _selectedSheetNameUse;
        public string SelectedSheetNameUse
        {
            get { return _selectedSheetNameUse; }
            set
            {
                if (_selectedSheetNameUse != value)
                {
                    _selectedSheetNameUse = value;
                    OnPropertyChanged(nameof(SelectedSheetNameUse));

                }
            }
        }
        public List<string> ItemsWildCard { get; set; }
        private string _selectedWildCard;
        public string SelectedWildCard
        {
            get { return _selectedWildCard; }
            set
            {
                if (_selectedWildCard != value)
                {
                    _selectedWildCard = value;
                    OnPropertyChanged(nameof(SelectedWildCard));
                }
            }
        }
        public List<string> ItemsCharactercode { get; set; }
        private string _selectedCharactercode;
        public string SelectedCharactercode
        {
            get { return _selectedCharactercode; }
            set
            {
                if (_selectedCharactercode != value)
                {
                    _selectedCharactercode = value;
                    OnPropertyChanged(nameof(SelectedCharactercode));
                }
            }
        }
        public List<string> ItemsHeaderUse { get; set; }
        private string _selectedHeaderUse;
        public string SelectedHeaderUse
        {
            get { return _selectedHeaderUse; }
            set
            {
                if (_selectedHeaderUse != value)
                {
                    _selectedHeaderUse = value;
                    OnPropertyChanged(nameof(SelectedHeaderUse));
                    OnHeaderLine = (SelectedHeaderUse == "あり") ? Visibility.Visible : Visibility.Hidden;

                }
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
        public List<string> ItemsThousandsSeparator { get; set; }
        private string _selectedThousandsSeparator;
        public string SelectedThousandsSeparator
        {
            get { return _selectedThousandsSeparator; }
            set
            {
                if (_selectedThousandsSeparator != value)
                {
                    _selectedThousandsSeparator = value;
                    OnPropertyChanged(nameof(SelectedThousandsSeparator));

                }
            }
        }
        public List<string> ItemsFirstMoney { get; set; }
        private string _selectedFirstMoney;
        public string SelectedFirstMoney
        {
            get { return _selectedFirstMoney; }
            set
            {
                if (_selectedFirstMoney != value)
                {
                    _selectedFirstMoney = value;
                    OnPropertyChanged(nameof(SelectedFirstMoney));
                    VisibilityFirstMoney = (SelectedFirstMoney == "何もしない") ? Visibility.Collapsed: Visibility.Visible;

                }
            }
        }
        public List<string> ItemsLastMoney { get; set; }
        private string _selectedLastMoney;
        public string SelectedLastMoney
        {
            get { return _selectedLastMoney; }
            set
            {
                if (_selectedLastMoney != value)
                {
                    _selectedLastMoney = value;
                    OnPropertyChanged(nameof(SelectedLastMoney));
                    VisibilityLastMoney = (SelectedLastMoney == "何もしない") ? Visibility.Collapsed: Visibility.Visible;

                }
            }
        }
        public List<string> ItemsAppend{ get; set; }
        private string _selectedAppend;
        public string SelectedAppend
        {
            get { return _selectedAppend; }
            set
            {
                if (_selectedAppend != value)
                {
                    _selectedAppend = value;
                    OnPropertyChanged(nameof(SelectedAppend));
                    VisibilityAppend = (SelectedAppend == "付加する") ? Visibility.Visible: Visibility.Collapsed;

                }
            }
        }
        public IProjectSetting NextStep()
        {
            switch (buttonStep)
            {
                case ButtonStep.NewTitleFile:
                    bool checkPrjName = CheckInput_PrjName(_projectName);
                    if (checkPrjName)
                    {
                        DownloadSpecify = Visibility.Visible;
                        ProjectTitle = Visibility.Visible;
                        buttonStep = ButtonStep.SpecifyFile;
                        SpecifyFile specifyFile = new SpecifyFile(this);
                        return specifyFile;
                    }
                    return null;

                case ButtonStep.SpecifyFile:
                    bool checkSrcFile = CheckInput_SrcFile(sourceFile);
                    if (checkSrcFile)
                    {
                        // Kiểm tra tệp tồn tại ở đây
                        if (!System.IO.File.Exists(sourceFile))
                        {
                            DFileReferenceError fileReferenceError = new DFileReferenceError(this);
                            return fileReferenceError;

                        }
                        if (SelectedFileFormat == "Excel")
                        {
                            if (IsRDONameSheetChecked)
                            {
                                if(SelectedSheetNameUse == null)
                                {
                                    SheetNameSetting = Visibility.Visible;
                                    return null;
                                }
                            }
                            else
                            {
                                SheetNameSetting = Visibility.Collapsed;
                            }
                        }

                        if (!CheckFileFormat(sourceFile))
                        {
                            CheckFileFormatLabel = Visibility.Visible;
                            return null;
                        }
                        else
                        {
                            CheckFileFormatDetail();
                            CheckFileFormatLabel = Visibility.Collapsed;
                            NextStepVisibility = Visibility.Collapsed;
                            ConversionEdit = Visibility.Visible;
                            buttonStep = ButtonStep.SourceFileDetailsSetting;
                            SourceFileDetailsSetting sourceFileDetailsSetting = new SourceFileDetailsSetting(this);
                            this.SaveProjectConfig();
                            if (!nextUserControlTable.ContainsKey(buttonStep)) {
                                nextUserControlTable.Add(buttonStep, new CurrencyDisplayedChanged(new AmountFormatChangeSettingVM()));
                            }

                            return sourceFileDetailsSetting;
                        }
                    }
                    return null;

                case ButtonStep.SourceFileDetailsSetting:
                    {
                        if (!nextUserControlTable.ContainsKey(buttonStep)) {
                            nextUserControlTable.Add(buttonStep, new CurrencyDisplayedChanged(new AmountFormatChangeSettingVM()));
                        }
                        var currencyDisplayedChanged = nextUserControlTable[buttonStep] as IProjectSetting;
                        buttonStep = ButtonStep.CurrencyDisplayedChange;
                        return currencyDisplayedChanged;
                    }
                default:
                    return null;
            }
        }

        public UserControl PreviousStep()
        {
            switch (buttonStep)
            {
                case ButtonStep.NewTitleFile:
                    return null;
                case ButtonStep.SpecifyFile:
                    DownloadSpecify = Visibility.Collapsed;
                    ProjectTitle = Visibility.Collapsed;
                    buttonStep = ButtonStep.NewTitleFile;
                    NewTitleFile newTitleFile = new NewTitleFile(this);
                    return newTitleFile;
                case ButtonStep.SourceFileDetailsSetting:
                    SpecifyFile _specifyFile = new SpecifyFile(this);
                    NextStepVisibility = Visibility.Visible;
                    ConversionEdit = Visibility.Collapsed;
                    buttonStep = ButtonStep.SpecifyFile;
                    return _specifyFile;
                case ButtonStep.CurrencyDisplayedChange:
                    SourceFileDetailsSetting sourceFileDetails = new SourceFileDetailsSetting(this);
                    buttonStep = ButtonStep.SourceFileDetailsSetting;
                    return sourceFileDetails;
                default:
                    // Trong trường hợp không xác định, không có bước trước đó để quay lại
                    return null;
            }
        }

        private bool CheckFileFormat(string fileFormat)
        {
            string fileExtension = System.IO.Path.GetExtension(fileFormat);

            string selectedFormat = ConvertFormatToExtension(SelectedFileFormat);

            return fileExtension.Equals(selectedFormat, StringComparison.OrdinalIgnoreCase);
        }
        private string ConvertFormatToExtension(string format)
        {
            switch (format)
            {
                case "CSV":
                    return ".csv";
                case "TSV":
                    return ".tsv";
                case "Excel":
                    return ".xlsx";
                case "Fix":
                    return ".dat";
                default:
                    return string.Empty;
            }
        }

        private void CheckFileFormatDetail()
        {
            string selectedFormat =SelectedFileFormat;
            switch(selectedFormat)
            {
                case "CSV":
                    NameFileConvert = "変換元テーブル : " + FilePathName + "-CSV";
                    break;
                case "TSV":
                    NameFileConvert = "変換元テーブル : " + FilePathName + "-TSV";
                    break;
                case "Excel":
                    SettingSourceExcel = Visibility.Visible;
                    if (IsRDONameSheetChecked)
                    {
                        NameFileConvert = "変換元テーブル : " + FilePathName + "-Excel" + " 【シート名称：" + SelectedSheetNameUse + "】";
                    }
                    else
                    {
                        NameFileConvert = "変換元テーブル : " + FilePathName + "-Excel" + " 【シート番号：" + ValueSheetNumber + "】";
                    }
                    break;
            }
        }

        private void FillPropertiesIntoProjectConfig()
        {
            this.projectConfigController.project.ProjectName = this.ProjectNameInput;
            this.projectConfigController.project.SourceFilePath = this.SourceFileSpecification;
            this.projectConfigController.project.ProjectMemo = this.projectNote;
        }
        private void SaveProjectConfig()
        {
            this.projectConfigController.SaveConfig();
            this.projectConfigController.OnProjectConfigChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
