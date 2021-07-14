using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class Home
    {
        public Home()
        {
            AppUserHomes = new HashSet<AppUserHome>();
            HomeImages = new HashSet<HomeImage>();
            Rooms = new HashSet<Room>();
            Things = new HashSet<Thing>();
        }

        public string HomeId { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Location { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public virtual ICollection<AppUserHome> AppUserHomes { get; set; }
        public virtual ICollection<HomeImage> HomeImages { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Thing> Things { get; set; }
    }
}
