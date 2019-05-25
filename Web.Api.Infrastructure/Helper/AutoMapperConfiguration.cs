using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Infrastructure.AutoMapperProfiles;

namespace Web.Api.Infrastructure.Helper
{
   public class AutoMapperConfiguration
    {

        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MstTestModelMapping>();
                cfg.AddProfile<MstUserModelMapping>();
                cfg.AddProfile<TestAthleteModelMapping>();
                cfg.AddProfile<SystemUsersModelMapping>();
                cfg.AddProfile<ErrorLogModelMapping>();
            });
        }
    }
}
