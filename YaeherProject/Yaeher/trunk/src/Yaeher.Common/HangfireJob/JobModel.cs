using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.Common.HangfireJob
{
  public  class JobModel
    {
        public string CallbackUrl;
        public string CallbackContent;
        public DateTime Timespan;
        public string queues;
    }
}
