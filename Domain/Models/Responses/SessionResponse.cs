using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class SessionResponse:IResponse
    {
        public string SessionId { get; set; }
        public double SessionUpdateTs { get; set; }
    }
}
