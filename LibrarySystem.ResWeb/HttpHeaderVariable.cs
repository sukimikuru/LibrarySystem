using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LibrarySystem.ResWeb
{
    public class HttpHeaderVariable
    {
        public bool IsContainsRange { get; set; }
        public ContentRange Range { get; set; }
        public bool IsContainsModify { get; set; }
        public DateTime SinceModify { get; set; }
        public bool IsContanisIfRange { get; set; }
        public string IfRangeString { get; set; }
        public bool IsContainsUnmodified { get; set; }
        public DateTime SinceUnmodify { get; set; }
        public static HttpHeaderVariable Init(HttpRequestBase request)
        {
            HttpHeaderVariable r = new HttpHeaderVariable();
            r.Range = new ContentRange();
            var ifRanges = request.Headers.GetValues("If-Range");
            if (ifRanges != null)
            {
                r.IsContanisIfRange = true;
                r.IfRangeString = ifRanges[0];
            }
            var ifModifies = request.Headers.GetValues("If-Modified-Since");
            if (ifModifies != null)
            {
                r.IsContainsModify = true;
                r.SinceModify = Convert.ToDateTime(ifModifies[0]);
            }
            var ifUnmodifies = request.Headers.GetValues("If-Unmodified-Since");
            if (ifUnmodifies != null)
            {
                r.IsContainsUnmodified = true;
                r.SinceUnmodify = Convert.ToDateTime(ifUnmodifies[0]);
            }
            var ranges = request.Headers.GetValues("Range");
            if (ranges != null)
            {
                r.IsContainsRange = true;
                var range = ranges[0];
                int indexD = range.IndexOf('=');
                int indexJ = range.IndexOf('-');
                r.Range.Start = Convert.ToInt32(range.Substring(indexD + 1, indexJ - indexD - 1));
                if (indexJ == range.Length - 1)
                    r.Range.End = 0;
                else
                    r.Range.End = Convert.ToInt32(range.Substring(indexJ + 1, range.Length - indexJ - 1));
            }
            return r;
        }
    }
}
