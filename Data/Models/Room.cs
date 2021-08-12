using Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Room: IdNameTsDataWithImages
    {
        public List<Cupboard> Cupboards { get; set; }
    }
}
