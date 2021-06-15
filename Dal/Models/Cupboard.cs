using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class Cupboard
    {
        public Cupboard()
        {
            Grids = new HashSet<Grid>();
        }

        public int CupboardId { get; set; }
        public string Name { get; set; }
        public int Wide { get; set; }
        public int Height { get; set; }
        public int RoomId { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public virtual Room Room { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<Grid> Grids { get; set; }
    }
}
