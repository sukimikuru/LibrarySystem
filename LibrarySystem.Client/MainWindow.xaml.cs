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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibrarySystem.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Search_Tool_Tip = "搜索资源...";


        public MainWindow()
        {
            InitializeComponent();
            imgBtnCheck(btnBookShell, null);

            InitSearchTextBox();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定要退出？", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }


        /// <summary>
        /// 搜索资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serach_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MessageBox.Show("开始搜索");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        /// <summary>
        /// 点击图标按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgBtnCheck(object sender, MouseButtonEventArgs e)
        {
            //先还原所有按钮的背景色
            Color unChecekdColor = Color.FromRgb(220, 25, 25);
            btnLibrary.Background = new SolidColorBrush(unChecekdColor);
            btnBookShell.Background = new SolidColorBrush(unChecekdColor);

            //设置选中背景色
            Color checekdColor = Color.FromRgb(164, 12, 12);
            (sender as Grid).Background = new SolidColorBrush(checekdColor);

            //mainContent.Content = new BookShellView();
            //leftContent.Content = new LeftView();

            //MessageBox.Show((sender as Grid).Tag.ToString());
        }

        private void searchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text.Trim() == Search_Tool_Tip)
            {
                searchTextBox.Text = "";
                Color white_color = Color.FromRgb(255, 255, 255);
                searchTextBox.Foreground = new SolidColorBrush(white_color);
            }
        }

        private void searchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text.Trim()))
            {
                InitSearchTextBox();
            }
        }

        private void InitSearchTextBox()
        {
            searchTextBox.Text = Search_Tool_Tip;
            Color gray_color = Color.FromRgb(220, 220, 220);
            searchTextBox.Foreground = new SolidColorBrush(gray_color);
        }

        private void headImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Login loginForm = new Login();

            bool? log_result = loginForm.ShowDialog();

            if (log_result == true)
            {
                MessageBox.Show("登陆成功,row_key=" + Utility.LoginUserInfo.LoginName);

                //更新ui

            }
            else if (log_result == false)
            {

                MessageBox.Show("取消登陆,row_key=" + Utility.LoginUserInfo.LoginName);
            }
        }
    }
}
