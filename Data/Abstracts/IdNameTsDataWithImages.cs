using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstracts
{
    public class IdNameTsDataWithImages:IdNameTsData
    {
        public List<string> ImagePaths { get; set; } = new List<string>();
    }
}
