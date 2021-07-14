using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class HomeImage
    {
        public int HomeImageId { get; set; }
        public string HomeId { get; set; }
        public string ImagePath { get; set; }

        public virtual Home Home { get; set; }
    }
}
