using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstracts
{
    public class IdNameTsData:IdNameData, ITsData
    {
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }
    }
}
