using Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Thing: IdNameTsDataWithImages
    {
        public string HouseId { get; set; }
        
        public int Quantity { get; set; }
    }
}
