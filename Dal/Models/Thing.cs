using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class Thing
    {
        public int ThingId { get; set; }
        public string HomeId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public virtual Home Home { get; set; }
    }
}
