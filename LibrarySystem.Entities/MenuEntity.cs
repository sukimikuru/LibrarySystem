using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Entities
{
    public class MenuEntity
    {
        /// <summary>
        /// key，唯一标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 展示名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 对应font-awesome图标的key
        /// </summary>
        public string Icon { get; set; }
        

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }
    }

}
