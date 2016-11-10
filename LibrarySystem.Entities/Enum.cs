using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LibrarySystem.Entities
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum SexType
    {
        [EnumMapping("1", "男")]
        Male,
        [EnumMapping("2", "女")]
        Women,
    }
    /// <summary>
    /// 角色（管理员、普通用户）
    /// </summary>
    public enum RoleKind
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [EnumMapping("1", "管理员")]
        Admin,
        /// <summary>
        /// 教务处
        /// </summary>
        [EnumMapping("2", "普通用户")]
        Normal
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResType
    {
        /// <summary>
        /// 文档
        /// </summary>
        [EnumMapping("1", "文档")]
        Doc,
        /// <summary>
        /// 视频
        /// </summary>
        [EnumMapping("2", "视频")]
        Vedio,
        /// <summary>
        /// 网页
        /// </summary>
        [EnumMapping("3", "网页")]
        Web,

    }

}
