using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Models
{
    public class Cupboard
    {
        public int CupboardId { get; set; }
        public Room Room { get; set; }
        public string Name { get; set; }
        public int Wide { get; set; }
        public int Height { get; set; }
        public int RoomId { get; set; }
        public List<Grid> Grids { get; } = new List<Grid>();
        public string ImagePath { get; set; }
    }
}
