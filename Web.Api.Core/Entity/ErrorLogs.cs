using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Entity
{
   public class ErrorLogs
    {
        public int id { get; set; }
        public string actioName { get; set; }
        public string logType { get; set; }
        public string description { get; set; }
    }
}
