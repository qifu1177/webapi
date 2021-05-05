using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Room
    {
        private List<Cupboard> _cupboards = new List<Cupboard>();
        public int RoomId { get; set; }
        public string Name { get; set; }
        public List<Cupboard> Cupboards {
            get { return _cupboards; }
            
        }
        public List<Dal.Models.Cupboard> Dal_Cupboard
        {
            set
            {
                _cupboards.Clear();
                value.ForEach(item => _cupboards.Add(new Cupboard { CupboardId = item.CupboardId, RoomId = item.RoomId, Name = item.Name, Wide = item.Wide, Height = item.Height }));
            }
        }
        public double TS { get; set; }
        public string ImagePath { get; set; }
    }
}
