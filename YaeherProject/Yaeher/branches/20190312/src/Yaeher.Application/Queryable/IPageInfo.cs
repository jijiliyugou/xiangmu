using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    public interface IPageInfo
    {
        long PageIndex { get; set; }

        uint PageSize { get; set; }
    }
}
