using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Models
{
    public class Grid
    {
        public int GridId { get; set; }
        public int CupboardId { get; set; }
        public Cupboard Cupboard { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public List<Thing> Things { get; } = new List<Thing>();
        public string ImagePath { get; set; }

    }
}
