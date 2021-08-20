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
        MessageResponse Update(string language, Request request);
        IdResponse Insert(string language, Request request);
        IEnumerable<Response> LoadAll(string language);
        IEnumerable<Response> Load(string language, object parentId);
        Response LoadWithId(string language, object id);
        MessageResponse DeleteWithId(string language, object id);
    }
}
