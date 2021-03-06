using Data.Abstracts;
using Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class AppUser:IdNameTsData
    {        
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; }
        public List<string> Houses { get; set; }
        public AppSession Session { get; set; }
        public string AccountUserId { get; set; }
        public UserInfo UserInfo { get; set; }

    }
    public class UserInfo
    {
        public string PhoneNumber { get; set; }
        public bool? MaritalStatus { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhotoPath { get; set; }
    }
}
