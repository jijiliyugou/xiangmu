using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    public class PageInfo :IPageInfo
    {
        public long PageIndex { get; set; }

        public uint PageSize { get; set; }
    }
}
