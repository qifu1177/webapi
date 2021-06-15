using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class RoleRight
    {
        public int RoleRightId { get; set; }
        public string RoleId { get; set; }
        public int ModuleId { get; set; }
        public string RoleRight1 { get; set; }

        public virtual Module Module { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
