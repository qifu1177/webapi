using BLL.Models;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface DBInterface
    {
        
        MessageObject Insert(Dictionary<string, StringValues> dic,DateTime updateTs);
        MessageObject Update(Dictionary<string, StringValues> dic, DateTime updateTs);
        MessageObject DeleteWithId(int id, DateTime updateTs);
                
    }
}
