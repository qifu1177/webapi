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
        Dictionary<string, JsonLocalization> _localizationDic = new Dictionary<string, JsonLocalization>();
        public Translator(string jsonStr)
        {
            List<JsonLocalization>  localization = JsonConvert.DeserializeObject<List<JsonLocalization>>(jsonStr);
            foreach(var item in localization)
            {
                _localizationDic[item.Key] = item;
            }
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
            if(_localizationDic.ContainsKey(name))
            {
                Dictionary<string, string> dic = _localizationDic[name].LocalizedValue;
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
