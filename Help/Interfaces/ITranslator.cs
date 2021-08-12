using System;
using System.Collections.Generic;
using System.Text;

namespace Help.Interfaces
{
    public interface ITranslator
    {
        string this[string language, string key] { get; }
        string this[string language, string key, params object[] auguments] { get; }
    }
}
