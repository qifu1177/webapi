using Data.Abstracts;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class AppSession : IdData, ITsData
    {
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }
    }
}
