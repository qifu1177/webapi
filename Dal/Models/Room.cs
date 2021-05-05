using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public List<Cupboard> Cupboards { get; } = new List<Cupboard>();
        public DateTime TS { get; set; }
        public string ImagePath { get; set; }
    }
}
