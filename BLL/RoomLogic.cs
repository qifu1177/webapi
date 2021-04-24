using BLL.Models;
using Dal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class RoomLogic
    {
        private string _conStr;
        public RoomLogic(string conStr)
        {
            _conStr = conStr;
        }
        public string All()
        {
            string str = "";
            List<Room> list = new List<Room>();
            using (DataContext context = new DataContext(_conStr))
            {
                list=context.Rooms.Include(r => r.Cupboards)
                        .Select(room=>new Room {RoomId=room.RoomId, Name=room.Name,TS=(room.TS-ConstentValue.JSZeroDt).TotalMilliseconds                         
                        }).ToList();
            }
            str= JsonConvert.SerializeObject(list, Formatting.None);
            return str;
        }

        private List<Cupboard> GetCupboards(List<Dal.Models.Cupboard> dbDataList)
        {
            List<Cupboard> list = new List<Cupboard>();
            dbDataList.ForEach(item => list.Add(new Cupboard { CupboardId = item.CupboardId, RoomId = item.RoomId, Name = item.Name, Wide = item.Wide, Height = item.Height }));
            return list;
        }
    }
}
