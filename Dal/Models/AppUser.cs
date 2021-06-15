using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class AppUser
    {
        public AppUser()
        {
            AppSessions = new HashSet<AppSession>();
            Cupboards = new HashSet<Cupboard>();
            Grids = new HashSet<Grid>();
            Rooms = new HashSet<Room>();
            Things = new HashSet<Thing>();
        }

        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }
        public string RoleId { get; set; }

        public virtual AppRole Role { get; set; }
        public virtual ICollection<AppSession> AppSessions { get; set; }
        public virtual ICollection<Cupboard> Cupboards { get; set; }
        public virtual ICollection<Grid> Grids { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Thing> Things { get; set; }
    }
}
