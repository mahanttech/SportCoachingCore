using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Infrastructure.Interface
{
    public interface IErrorLog
    {
        void BindErrorLogModel(string action, string description, string logType);
     }
}
