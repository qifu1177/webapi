using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class EnumData
    {
        public enum AppModules
        {
            AppUser,
            AppRole,
            House,
            Thing,
            GridThings
        }
        public enum RoleRight
        {
            all,
            account,
            user,
            no
        }
        public enum Role
        {
            Admin,
            Manager,
            User
        }
    }
}
