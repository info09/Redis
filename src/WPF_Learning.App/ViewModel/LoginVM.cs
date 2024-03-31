using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Learning.Core.Domain.Identity;

namespace WPF_Learning.App.ViewModel
{
    public class LoginVM : BaseViewModel
    {
        //private readonly UserManager<AppUser> _userManager;
        //private readonly SignInManager<AppUser> _signInManager;
        public bool IsLogin { get; set; }

        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public LoginVM()
        {
            //_userManager = userManager;
            //_signInManager = signInManager;
            IsLogin = false;
            Password = "";
            UserName = "";
            //LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }

        private async Task Login(Window p)
        {
            //if (p == null)
            //    return;

            //var user = await _userManager.FindByNameAsync(UserName);

            //if (user == null)
            //{
            //    IsLogin = false;
            //    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            //    return;
            //}
            //var result = await _signInManager.PasswordSignInAsync(user, Password, true, true);

            //if(result.Succeeded)
            //{
            //    IsLogin = true;

            //    p.Close();
            //}
            //else
            //{
            //    IsLogin = false;
            //    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            //}
        }
    }
}
