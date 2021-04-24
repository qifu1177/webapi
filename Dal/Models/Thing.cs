using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Models
{
    public class Thing
    {
        public int ThingId { get; set; }
        public int GridId { get; set; }
        public Grid Grid { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
