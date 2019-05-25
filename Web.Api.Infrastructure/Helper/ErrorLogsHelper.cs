using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Context;
using Web.Api.Core.Entity;
using Web.Api.Infrastructure.Interface;
using Web.Api.Infrastructure.Models;

namespace Web.Api.Infrastructure.Helper
{
    public class ErrorLogsHelper:IErrorLog
    {
        private SportTestContext _context;
        private readonly IMapper _mapper;

        public ErrorLogsHelper(SportTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void BindErrorLogModel(string action,string description, string logType)
        {
            ErrorLogModel errorLogModel = new ErrorLogModel();
            errorLogModel.actioName = action;
            errorLogModel.description = description;
            errorLogModel.logType = logType;
            AddLog(errorLogModel);
        }

        public void AddLog(ErrorLogModel errorLogModel)
        {
            ErrorLogs _errorLogs = new ErrorLogs();
            _errorLogs = Mapper.Map<ErrorLogModel, ErrorLogs>(errorLogModel);
            _context.ErrorLogs.Add(_errorLogs);
            _context.SaveChanges();
        }


    }
}
