using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime JSZeroDt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static double ToJsTime(this DateTime dt)
        {
            return (dt - DateTimeExtensions.JSZeroDt).TotalMilliseconds;
        }
    }
}
