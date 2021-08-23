using Help.Constents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static string ToDateString(this DateTime dt )
        {
            return string.Format("{0:yyyy-MM-dd}", dt);
        }
        public static DateTime? ToDate(string str)
        {
            if(!string.IsNullOrEmpty(str) && Regex.IsMatch(str, RegexStrings.DateStringExpression))
            {
                string[] s = str.Split('-');
                return new DateTime(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2]));
            }
            
            return null;
        }
    }
}
