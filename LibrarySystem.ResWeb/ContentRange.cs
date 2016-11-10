using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystem.ResWeb
{
    public class ContentRange
    {
        public long Start { get; set; }
        public long End { get; set; }
        public long Total { get; set; }
        public long Length { get { return End - Start + 1; } }
        public ContentRange(long start, long end, long total)
        {
            Start = start;
            End = end;
            Total = total;
        }
        public ContentRange()
        {
        }
    }
}
