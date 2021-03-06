using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class UserSessionResponse: SessionResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public double UpdateTs { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public bool? MaritalStatus { get; set; }
        public string BirthDate { get; set; }
        public string PhotoPath { get; set; }
        public double MaxUploadFileSize { get; set; } = 0;
    }
}
