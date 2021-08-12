using BLL.Models.Helps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BLL.Helps
{   
    public class StringLocalizer
    {
        List<JsonLocalization> localization = new List<JsonLocalization>();
        public StringLocalizer(Assembly assembly, string namespaceAndFileName)
        {
            string json = EmbeddedResource.GetApiRequestFile(assembly, namespaceAndFileName);
            SetJson(json);
        }

        public StringLocalizer(string jsonStr)
        {
            SetJson(jsonStr);
        }

        private void SetJson(string jsonStr)
        {
            localization = JsonConvert.DeserializeObject<List<JsonLocalization>>(jsonStr);
        }
        public string this[string language,string name]
        {
            get
            {
                return GetString(language,name);
            }
        }

        public string this[string language,string name, params object[] arguments]
        {
            get
            {
                var format = GetString(language,name);
                var value = string.Format(format ?? name, arguments);
                return value;
            }
        }     

        private string GetString(string language,string name)
        {
            var query = localization.Where(l => l.LocalizedValue.Keys.Any(lv => lv == language));
            var value = query.FirstOrDefault(l => l.Key == name);
            if (value == null || !value.LocalizedValue.ContainsKey(language))
            {
                return name;
            }
            return value.LocalizedValue[language];
        }

    }
}
