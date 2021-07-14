using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ConstentValue
    {
        public const string EMAIL_PATTERN= @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        public static DateTime JSZeroDt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //Error Message
        public const string USER_LOGIN_EMAIL_OR_PASSWORD_ERROR= "EMAIL_OR_PASSWORD_ERROR";
        public const string USER_REGISTER_EMAIL_ERROR = "EMAIL_EXIST_ERROR";
        public const string USER_REGISTER_ROLE_ERROR = "EMAIL_ROLE_ERROR";
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
    public static class CollectionExtensions
    {
        public static ICollection<T> Adds<T>(this ICollection<T> collection, params T[] args)
        {
            foreach (T value in args)
                collection.Add(value);
            return collection;
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
