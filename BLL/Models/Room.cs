using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public List<Cupboard> Cupboards { get; set; } = new List<Cupboard>();
        public double TS { get; set; }
    }
}
