using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystem.Entities
{
    public class CookieInfo
    {
        public long UserKey { get; set; }

        public Guid ValidKey { get; set; }

        public DateTime LastTime { get; set; }
    }
}
