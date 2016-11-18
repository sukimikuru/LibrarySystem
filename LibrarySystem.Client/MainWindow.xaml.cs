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
using LibrarySystem.ResWeb;

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
            //imgBtnCheck(btnBookShell, null);

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


            LibraryService.ILibrary _service = new LibraryService.LibraryClient();
            LibraryService.ResEntity[] resList = _service.ResPagerList(1, 30, null, null);

            this.DataContext = new MainViewModel(resList);

            //mainContent.Content = new ListBox();
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
                MessageBox.Show("登陆成功,row_key=" + CommonConfig.Current.LoginUserInfo.LoginName);

                //更新ui
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = GetBitmapImage(CommonConfig.Current.LoginUserInfo.HeadImg);
                headImgFill.Fill = ib;

                userNameLabel.Content = CommonConfig.Current.LoginUserInfo.LoginName;

            }
            else if (log_result == false)
            {

                MessageBox.Show("取消登陆,row_key=" + CommonConfig.Current.LoginUserInfo.LoginName);
            }
        }


        /// <summary>
        /// 根据图片的相对路径 返回 BitmapImage对象的实例化
        /// </summary>
        /// <param name="imgPath">图片的相对路径(如:@"/images/star.png")</param>
        /// <returns></returns>
        public static BitmapImage GetBitmapImage(string imgPath)
        {
            try
            {
                imgPath = "localhost:24476" + RWUtility.FormatResUrl(imgPath, ResFileType.UserIcon28x28);
                if (!imgPath.StartsWith("/"))
                {
                    imgPath = "/" + imgPath;
                }
                return new BitmapImage(new Uri("Pack://application:,,," + imgPath));
            }
            catch (Exception ex)
            {
                return new BitmapImage(new Uri(@"头像.png", UriKind.Relative)); ;
            }
        }


        /// <summary>
        /// 点击图书项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                string res_key = "0";
                Grid grid = sender as Grid;
                foreach (var c in grid.Children)
                {
                    if (c is Image && c != null)
                    {
                        res_key = (c as Image).Tag as string;
                        break;
                    }

                }

                ResDetail detailForm = new ResDetail(long.Parse(res_key));
                detailForm.ShowDialog();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                Grid element = sender as Grid;

                ContextMenu cm = new System.Windows.Controls.ContextMenu();
                element.ContextMenu = cm;

                {
                    MenuItem item = new MenuItem();
                    item.Header = "下载";
                    //item.Click += new RoutedEventHandler(item_Click);
                    cm.Items.Add(item);
                }

                {
                    MenuItem item = new MenuItem();
                    item.Header = "收藏";
                    //item.Click += new RoutedEventHandler(item_Click);
                    cm.Items.Add(item);
                }

                {
                    MenuItem item = new MenuItem();
                    item.Header = "阅读";
                    //item.Click += new RoutedEventHandler(item_Click);
                    cm.Items.Add(item);
                }

            }
        }
    }
}
