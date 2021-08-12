using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Exceptions
{
    public class TranslationException:Exception
    {
        public TranslationException(ITranslator translator, string language, string messageKey, Exception ex):base(translator[language, messageKey],ex)
        {            
        }
        public TranslationException(ITranslator translator, string language, string messageKey, Exception ex,params object[] auguments) : base(translator[language, messageKey,auguments], ex)
        {
        }
    }
}
