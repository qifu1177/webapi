using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class AppSession
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public virtual AppUser User { get; set; }
    }
}
