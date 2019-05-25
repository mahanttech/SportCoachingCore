using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Entity;
using Web.Api.Infrastructure.Helper;
using Web.Api.Infrastructure.Models;

namespace Web.Api.Infrastructure.AutoMapperProfiles
{
    public class TestAthleteModelMapping : Profile
    {
        public TestAthleteModelMapping()
        {
            CreateMap<TestAthleteMapping, TestAthleteMappingModel>()
                .ForMember(x => x.testId, y => y.Condition(z => (z.testId>0)));
        }
    }
}
