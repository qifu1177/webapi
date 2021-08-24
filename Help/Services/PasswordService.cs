

using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Help.Services
{
    public class PasswordService : IPasswordService
    {
        private const string DigitList = "1234567890";
        private const string LowerCharList = "qwertzuioplkjhgfdsayxcvbnm";
        private readonly string UpperCharList = LowerCharList.ToUpper();
        private const string SpecialSymbolList = @"!§$%&/()=?*#,.;:";
        public PasswordService() { }
        public string Generate()
        {
            string[] array = new string[] { DigitList, LowerCharList, UpperCharList, SpecialSymbolList };
            List<int> addedTypes = new List<int>();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            int length = random.Next(8, 12);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, array.Length);
                if (addedTypes.Count < array.Length)
                {
                    while (addedTypes.Contains(index))
                        index = random.Next(0, array.Length);
                    addedTypes.Add(index);
                }
                string str = array[index];
                int strIndex = random.Next(0, str.Length);
                builder.Append(str[strIndex]);
            }
            return builder.ToString();
        }
    }
}
