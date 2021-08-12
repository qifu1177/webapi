using Data.Abstracts;
using System.Collections.Generic;

namespace Data.Models
{
    public class AppRole:IdNameData
    {
        public Dictionary<string,string> ModuleRights { get; set; }
    }
}
