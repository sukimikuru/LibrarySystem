using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibrarySystem.Entities;

namespace LibrarySystem.Client
{
    public class MainViewModel
    {
        public MainViewModel(LibraryService.ResEntity[] resList)
        {
            this._Images = new List<String>();
            foreach (LibraryService.ResEntity item in resList)
            {
                string img_path = item.Img;
                if (string.IsNullOrEmpty(img_path))
                {
                    if (item.Type == ResType.Doc.GetDBCode())
                        img_path = "/Resources/video_img.png";
                    else if (item.Type == ResType.Vedio.GetDBCode())
                        img_path = "/Resources/book_img.png";
                    else if (item.Type == ResType.Web.GetDBCode())
                        img_path = "/Resources/web_img.png";

                    img_path += "|" + item.Type + "|1|" + item.Name + "|" + item.RowKey; ;
                }

                this._Images.Add(img_path);
            }

            //using (var sr = new StreamReader("list.txt"))
            //{
            //    this._Images = new List<String>();
            //    while (!sr.EndOfStream)
            //    {
            //        this._Images.Add(sr.ReadLine());
            //    }
            //}
        }
        private List<String> _Images;

        public List<String> Images
        {
            get { return _Images; }
            set { _Images = value; }
        }

    }
}
