using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class Grid
    {
        public int GridId { get; set; }
        public int CupboardId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public virtual Cupboard Cupboard { get; set; }
        public virtual AppUser User { get; set; }
    }
}
