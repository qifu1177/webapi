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
            AppUserHomes = new HashSet<AppUserHome>();
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
        public virtual ICollection<AppUserHome> AppUserHomes { get; set; }
    }
}
