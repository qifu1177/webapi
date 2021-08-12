using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class EnumData
    {
        public enum RoleRight
        {
            all,
            write,
            read,
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
