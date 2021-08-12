using Help.Datas;
using Help.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Help
{
    public class Translator : ITranslator
    {
        List<JsonLocalization> localization = new List<JsonLocalization>();
        public Translator(string jsonStr)
        {
            localization = JsonConvert.DeserializeObject<List<JsonLocalization>>(jsonStr);

        }
        public Translator(Assembly assembly, string namespaceAndFileName):this(EmbeddedResource.GetApiRequestFile(assembly,namespaceAndFileName))
        {
        }
        public string this[string language, string key]
        {
            get
            {
                return GetString(key, language);
            }
        }

        public string this[string language, string key, params object[] auguments] {
            get
            {
                var format = GetString(key, language);
                return string.Format(format ?? key, auguments);
            }
        }

        private string GetString(string name, string language = "en")
        {
            var query = localization.Where(l => l.Key==name);
            if(query.Count()>0)
            {
                Dictionary<string, string> dic = query.First().LocalizedValue;
                if (dic.ContainsKey(language))
                    return dic[language];
                else
                {
                    if (dic.Values.Count > 0)
                        return dic.Values.First();
                }
            }
            return name;
        }
    }
}
