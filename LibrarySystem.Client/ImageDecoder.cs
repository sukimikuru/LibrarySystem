using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace LibrarySystem.Client
{
    public static class ImageDecoder
    {
        public static readonly DependencyProperty SourceProperty;
        public static string GetSource(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("Image");
            }
            return (string)image.GetValue(ImageDecoder.SourceProperty);
        }
        public static void SetSource(Image image, string value)
        {
            if (image == null)
            {
                throw new ArgumentNullException("Image");
            }
            image.SetValue(ImageDecoder.SourceProperty, value);
        }
        static ImageDecoder()
        {
            ImageDecoder.SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(string), typeof(ImageDecoder), new PropertyMetadata(new PropertyChangedCallback(ImageDecoder.OnSourceWithSourceChanged)));
            ImageQueue.OnComplate += new ImageQueue.ComplateDelegate(ImageDecoder.ImageQueue_OnComplate);
        }
        private static void ImageQueue_OnComplate(Image i, string u, BitmapImage b, string title, string res_key)
        {
            //System.Windows.MessageBox.Show(u);
            string source = ImageDecoder.GetSource(i);
            //if (source == u.ToString())
            if (true)
            {
                i.Source = b;
                i.Tag = res_key;


                Storyboard storyboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromMilliseconds(500.0)));
                Storyboard.SetTarget(doubleAnimation, i);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity", new object[0]));
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
                if (i.Parent is Grid)
                {
                    Grid grid = i.Parent as Grid;
                    foreach (var c in grid.Children)
                    {
                        if (c is WaitingProgress && c != null)
                        {
                            (c as WaitingProgress).Stop();

                        }
                        else if (c is Label && c != null)
                        {
                            (c as Label).Content = title;
                        }
                    }
                }
            }
        }
        private static void OnSourceWithSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            string allpath = (string)e.NewValue;
            string[] result = allpath.Split('|');
            string url = result[0];
            string resType = result.Length > 1 ? result[1] : "0";
            bool localimage = result.Length > 2 ? (allpath.Split('|')[2] == "1") : true;
            string title = result.Length > 3 ? result[3] : "找不到名字";
            string resKey = result.Length > 4 ? result[4] : "0";

            ImageQueue.Queue((Image)o, url, localimage, title, resKey, resType);
        }
    }
}
