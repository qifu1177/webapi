using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Constents
{
    public class RegexStrings
    {
        public const string EmailFormExpression = @".+@{1}\w+\.{1}\w{2,}";
        public const string AdressFormExpression = @".+\d+\s*\,*\s*\d{5}.*";
    }
}
