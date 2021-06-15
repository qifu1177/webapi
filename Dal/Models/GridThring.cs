using System;
using System.Collections.Generic;

#nullable disable

namespace Dal.Models
{
    public partial class GridThring
    {
        public int GridId { get; set; }
        public int ThingId { get; set; }

        public virtual Grid Grid { get; set; }
        public virtual Thing Thing { get; set; }
    }
}
