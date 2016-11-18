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
using System.IO;
using System.Windows.Xps.Packaging;

namespace LibrarySystem.Client
{
    /// <summary>
    /// ResDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ResDetail : Window
    {
        private static long _res_key = 0;

        public ResDetail(long res_key)
        {
            _res_key = res_key;
            InitializeComponent();
            init();
        }

        public void init()
        {
            label.Content = _res_key;


            //string paht = @"E:\图书馆项目\文档\标准展示图书馆项目说明书.xps";

            //XpsDocument doc = new XpsDocument(paht, System.IO.FileAccess.Read);
            //documentViewer.Document = doc.GetFixedDocumentSequence();


            //MediaElement media = new MediaElement();
            //mediaElement.LoadedBehavior = MediaState.Manual;
            //mediaElement.Source = new Uri(@"E:\图书馆项目\文档\oceans.mp4");
           
            mediaElement.Source = new Uri("http://vjs.zencdn.net/v/oceans.mp4");
            mediaElement.Play();
            //mediaElement.Play();

        }


    }
}
