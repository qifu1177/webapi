using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Helps
{
    public class JsonLocalization
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedValue = new Dictionary<string, string>();

    }
}
