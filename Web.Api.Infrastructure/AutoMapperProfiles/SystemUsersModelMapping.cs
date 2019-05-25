using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Entity;
using Web.Api.Infrastructure.Models;

namespace Web.Api.Infrastructure.AutoMapperProfiles
{
   public class SystemUsersModelMapping : Profile
    {
        public SystemUsersModelMapping()
        {
            CreateMap<SystemUsers, SystemUsersModel>();
            CreateMap<SystemUsersModel, SystemUsers>();

        }
    }
}
