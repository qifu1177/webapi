using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Thing
    {
        public int ThingId { get; set; }
        public int GridId { get; set; }
       
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
    }
}
