using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ConstentValue
    {
        public static DateTime JSZeroDt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
    public static class DateTimeExtensions
    {
        public static double ToJsTime(this DateTime dt)
        {
            return (dt - ConstentValue.JSZeroDt).TotalMilliseconds;
        }

        public static DateTime CreateFromJsTime(double jsTime)
        {
            return ConstentValue.JSZeroDt.AddMilliseconds(jsTime);
        }
    }
    public static class StringExtensions
    {
        public static string Reverse(this string str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}
