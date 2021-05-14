using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class FileResult
    {
        public string Name { get; set; }
        public bool IsImage { get; set; }
        public string FileType { get; set; }
        public long Size { get; set; }
    }
}
