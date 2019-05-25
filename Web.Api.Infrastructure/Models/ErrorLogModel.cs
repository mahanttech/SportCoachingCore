using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Infrastructure.Models
{
   public class ErrorLogModel
    {
        public int id { get; set; }
        public string actioName{ get; set; }
        public string logType{ get; set; }
        public string description { get; set; }
    }
}
