using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Learning.App.ViewModel
{
    public class ControlBarVM : BaseViewModel
    {
        #region Command
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        #endregion

        public ControlBarVM()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });

            MinimizeWindowCommand = new RelayCommand<UserControl>(p => true, p =>
            {
                var window = GetWindowParent(p);
                Window? w = window as Window;
                if (w != null)
                {
                    if (w.WindowState != WindowState.Minimized)
                        w.WindowState = WindowState.Minimized;
                    else
                        w.WindowState = WindowState.Maximized;
                }
            });

            MaximizeWindowCommand = new RelayCommand<UserControl>(p => true, p =>
            {
                var window = GetWindowParent(p);
                Window? w = window as Window;
                if (w != null)
                {
                    if (w.WindowState != WindowState.Maximized)
                        w.WindowState = WindowState.Maximized;
                    else
                        w.WindowState = WindowState.Normal;
                }
            });

            MouseMoveWindowCommand = new RelayCommand<UserControl>(p => true, p =>
            {
                var window = GetWindowParent(p);
                Window? w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
        }

        FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement? parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }
    }
}
