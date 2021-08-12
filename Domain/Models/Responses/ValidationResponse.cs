using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class ValidationResponse
    {
        public List<string> Messages { get; set; } = new List<string>();
    }
}
