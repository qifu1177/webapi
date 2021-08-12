using Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Grid: IdNameTsDataWithImages
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        
    }
}
