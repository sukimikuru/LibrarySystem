using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace LibrarySystem.Entities
{ 
    public class UploadImageModel
    {
        public string headFileName { get; set; }
     
        public int x { get; set; }

       
        public int y { get; set; }

       
        public int width { get; set; }

       
        public int height { get; set; }
    }
}