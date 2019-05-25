using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Context;
using Web.Api.Core.Entity;
using Web.Api.Infrastructure.Models;

namespace Web.Api.Infrastructure.AutoMapperProfiles
{
    public class MstTestModelMapping : Profile
    {
     
        public MstTestModelMapping()
        {
            CreateMap<MstTest, MstTestModel>();
        }
    }
}
