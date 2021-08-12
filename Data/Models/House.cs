using Data.Abstracts;
using System.Collections.Generic;

namespace Data.Models
{
    public class House: IdNameTsDataWithImages
    {
       
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Location { get; set; }        

        public List<Room> Rooms { get; set; }
        public List<string> Users { get; set; }

    }
}
