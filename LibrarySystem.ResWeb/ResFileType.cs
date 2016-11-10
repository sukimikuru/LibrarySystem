using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystem.ResWeb
{
    /// <summary>
    /// 不同类型的ResFile对象(可用此来区分不同类型的缺省值的不同)
    /// </summary>
    public enum ResFileType
    {
        UserIcon,
        UserIcon120x120,
        UserIcon94x94,
        UserIcon48x48,
        UserIcon28x28,
        CourceIcon,
        ResFile,
        ResFile100x130,
        CommonFile,
        DiscussLogo120x120,
        DiscussLogo100x100,
        DiscussLogo50x50,
        NewsImage200x154,
        Book118x118,
    }
}
