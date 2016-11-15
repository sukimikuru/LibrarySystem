using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;


namespace LibrarySystem.Client
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {



        public Login()
        {
            InitializeComponent();
            Init();

        }

        private void Init()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            label_msg.Content = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            label_msg.Content = "";
            if (string.IsNullOrEmpty(usernameBox.Text.Trim()))
            {
                label_msg.Content = "用户名不能为空!";
                return;
            }

            if (string.IsNullOrEmpty(passwordBox.Password.Trim()))
            {
                label_msg.Content = "密码不能为空!";
                return;
            }

            loginBtn.Content = "登录中...";

            LibraryService.UserEntity logUser = new LibraryService.UserEntity();
            logUser.LoginName = usernameBox.Text.Trim();
            logUser.Pwd = passwordBox.Password.Trim();
            Thread newThread = new Thread(new ParameterizedThreadStart(LoginSystem));
            newThread.Start(logUser);
        }


        private void LoginSystem(object logUser)
        {
            try
            {
                string msg = "";
                LibraryService.UserEntity userInfo = new LibraryService.UserEntity();
                

                
                bool log_result = false;

                LibraryService.ILibrary _service = new LibraryService.LibraryClient();
                
                log_result = _service.LoginSys(out msg, out userInfo, (logUser as LibraryService.UserEntity).LoginName, (logUser as LibraryService.UserEntity).Pwd);
                
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    loginBtn.Content = "登录";

                    if (log_result)
                    {
                        Utility.LoginUserInfo = userInfo;
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        label_msg.Content = msg;
                        return;
                    }
                });


            }
            catch (Exception ex)
            {

            }

        }

    }
}
