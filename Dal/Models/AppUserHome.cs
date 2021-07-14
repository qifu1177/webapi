using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class AppUserHome
    {
        public int AppUserHomeId { get; set; }
        public string HomeId { get; set; }
        public string UserId { get; set; }

        public virtual Home Home { get; set; }
        public virtual AppUser User { get; set; }
    }
}
