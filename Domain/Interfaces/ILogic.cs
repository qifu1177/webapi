using Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILogic<Request,Response>where Request:IRequest where Response: IResponse
    {
        MessageResponse Update(string language, string sessionId, Request request);
        IdResponse Insert(string language, string sessionId, Request request);
        IEnumerable<Response> LoadAll(string language, string sessionId);
        IEnumerable<Response> Load(string language, string sessionId, object parentId);
        Response LoadWithId(string language, string sessionId, object id);
        MessageResponse DeleteWithId(string language, string sessionId, object id);
    }
}
