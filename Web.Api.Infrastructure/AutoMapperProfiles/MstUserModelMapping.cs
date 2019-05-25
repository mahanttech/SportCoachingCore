using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Entity;
using Web.Api.Infrastructure.Helper;
using Web.Api.Infrastructure.Models;
namespace Web.Api.Infrastructure.AutoMapperProfiles
{
   public class MstUserModelMapping : Profile
    {
        public MstUserModelMapping()
        {
            CreateMap<MstUser, MstUserModel>()
                .ForMember(x=>x.Role,y=>y.Condition(z=>(z.Role==Constants.role)));
        }
    }
}
