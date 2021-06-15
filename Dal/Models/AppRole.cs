using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class AppRole
    {
        public AppRole()
        {
            AppUsers = new HashSet<AppUser>();
            RoleRights = new HashSet<RoleRight>();
        }

        public string RoleId { get; set; }
        public string RoleDesc { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }
        public virtual ICollection<RoleRight> RoleRights { get; set; }
    }
}
