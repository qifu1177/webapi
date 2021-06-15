using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class Module
    {
        public Module()
        {
            RoleRights = new HashSet<RoleRight>();
        }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }

        public virtual ICollection<RoleRight> RoleRights { get; set; }
    }
}
