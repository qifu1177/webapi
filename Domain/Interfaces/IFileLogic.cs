using Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFileLogic
    {
        string GetUserId(string language, string sessionId);
    }
}
