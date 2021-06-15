using BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Room
    {
        private string _link = "";
        private string _icon = "";
        private string _fileType = "";
        private bool _isImage = false;
        private string _fileDir = "";
        private string _baseUrl = "";
        private string _imagePath = "";
        public string FileDir { set { _fileDir = value; } }
        public string BaseUrl { set { _baseUrl = value; } }
        private List<Cupboard> _cupboards = new List<Cupboard>();
        public int RoomId { get; set; }
        public string Name { get; set; }
        public List<Cupboard> Cupboards
        {
            get { return _cupboards; }

        }
        public ICollection<Dal.Models.Cupboard> Dal_Cupboard
        {
            set
            {
                _cupboards.Clear();
                foreach (var item in value)
                {
                    _cupboards.Add(new Cupboard { CupboardId = item.CupboardId, RoomId = item.RoomId, Name = item.Name, Wide = item.Wide, Height = item.Height });
                }

            }
        }
        public double TS { get; set; }
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                FileService.Instance.GetFielInfo(_fileDir, _baseUrl, _imagePath, out _link, out _icon, out _isImage, out _fileType);

            }
        }
        public string Link { get { return _link; } }
        public string Icon { get { return _icon; } }
        public string FileType { get { return _fileType; } }
        public bool IsImage { get { return _isImage; } }
    }
}
