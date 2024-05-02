using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace NewIDC.App.Models.LibraryModels
{
    public enum HeaderInputType
    {
        None = -1,
        Combobox = 0,
        Checkbox = 1,
        Textbox = 2,
    }
    public class HeaderInput
    {
        public HeaderInputType HeaderType { get; set; }
        public ICollection Data { get; set; }
    }
    public class HeaderInputData
    {
        public int ColumnIndex { get; set; }
    }
    public class ComboboxHeaderInput : HeaderInputData
    {
        public List<ComboboxItem> Items { get; set; }
        public string SelectedValue { get; set; }
        public string TextBelow { get; set; }
    }
    public class ComboboxItem
    {
        public string Displayed { get; set; }
        public string Value { get; set; }
    }
    public class CheckboxHeaderInput : HeaderInputData
    {
        public bool IsChecked { get; set; }
        public string CheckboxText { get; set; }
    }
    public class TextErrorHeaderInput : HeaderInputData
    {
        public string Text { get; set; }
    }
    public class RowHeader : TextErrorHeaderInput
    {
        public string IconPath { get; set; }
    }
    public class CellDataInput
    {
        public double Width { get; set; } = 100;
        public Visibility ComboboxVisibility { get; set; } = Visibility.Collapsed;
        public Visibility HienThi { get; set; } = Visibility.Collapsed;
        public ICollection ComboboxItems { get; set; } = null;
        public string ComboboxSelectedValue { get; set; } = string.Empty;
        public Visibility TextBlockVisibility { get; set; } = Visibility.Collapsed;
        public string TextBlockInput { get; set; } = string.Empty;
        public SolidColorBrush ForeColor { get; set; } = Brushes.Black;
        public SolidColorBrush BackColor { get; set; } = Brushes.White;
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
        public Visibility CheckboxVisibility { get; set; } = Visibility.Collapsed;
        public bool IsChecked { get; set; } = false;
        public string CheckboxText { get; set; } = "";
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;
        public bool IsHeader { get; set; }
        public static CellDataInput EmptyDisplayed(SolidColorBrush ForeColor, SolidColorBrush BackColor)
        {
            var cell = new CellDataInput()
            {
                TextBlockVisibility = Visibility.Visible,
                TextAlignment = TextAlignment.Center,
                ForeColor = ForeColor,
                BackColor = BackColor
            };
            return cell;
        }
        public static CellDataInput ComboboxDisplayed(ICollection Items, string SelectedValue, double Width = 100)
        {
            var cell = new CellDataInput()
            {
                Width = Width,
                ComboboxVisibility = Visibility.Visible,
                ComboboxItems = Items,
                ComboboxSelectedValue = SelectedValue,
            };
            return cell;
        }
        public static CellDataInput TextBlockDisplayed(string Input, SolidColorBrush ForeColor, SolidColorBrush BackColor, TextAlignment TextAlignment, FontWeight FontWeight, double Width = 100)
        {
            var cell = new CellDataInput()
            {
                Width = Width,
                TextBlockVisibility = Visibility.Visible,
                TextBlockInput = Input,
                ForeColor = ForeColor,
                BackColor = BackColor,
                TextAlignment = TextAlignment,
                FontWeight = FontWeight
            };
            return cell;
        }
        public static CellDataInput TextBlockHeaderDisplayed(string Input, double Width = 100)
        {
            var cell = new CellDataInput()
            {
                Width = Width,
                TextBlockVisibility = Visibility.Visible,
                TextBlockInput = Input,
                ForeColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666")),
                BackColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE599")),
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold,
            };
            return cell;
        }
        public static CellDataInput CheckboxDisplayed(bool IsChecked, string CheckboxText, double Width = 100)
        {
            var cell = new CellDataInput()
            {
                Width = Width,
                CheckboxVisibility = Visibility.Visible,
                CheckboxText = CheckboxText,
                IsChecked = IsChecked,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            return cell;
        }
    }
}
