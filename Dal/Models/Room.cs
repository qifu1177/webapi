﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class Room
    {
        public Room()
        {
            Cupboards = new HashSet<Cupboard>();
        }

        public int RoomId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string HomeId { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public virtual Home Home { get; set; }
        public virtual ICollection<Cupboard> Cupboards { get; set; }
    }
}
