using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Entity;
using Web.Api.Infrastructure.Interface;
using Web.Api.Infrastructure.Models;
using System.Linq;
using Web.Api.Infrastructure.Helper;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using Web.Api.Core.Context;

namespace Web.Api.Infrastructure.Repository
{
    public class TestRepository : ITestRepository
    {
        private SportTestContext _context;
        private readonly IMapper _mapper;
        private readonly IErrorLog _errorLog;

        public TestRepository(SportTestContext context, IMapper mapper, IErrorLog errorLog)
        {
            _context = context;
            _mapper = mapper;
            _errorLog = errorLog;
        }

        public List<MstTestModel> GetTestList()
        {
            try
            {
                return _context.MstTest.Select(x => new MstTestModel()
                {
                    Id = x.Id,
                    NoOfParticipant = _context.TestAthleteMapping.Where(y => y.testId == x.Id).Count(),
                    TestDate = x.TestDate,
                    TestType = x.TestType
                }).OrderByDescending(x=>x.TestDate).ToList();
            }
            catch (Exception ex)
            {

                _errorLog.BindErrorLogModel("GetTestList", ex.Message, "error");
                return null;
            }
        }

        public int InsertUpdateTest(MstTestModel mstTestModel)
        {
            try
            {
                MstTest _mstTest = new MstTest();
                mstTestModel.TestDate = mstTestModel.TestDate.Value.AddDays(1);
                _mstTest = _context.MstTest.Where(x => x.Id == mstTestModel.Id).FirstOrDefault();
                _mstTest = Mapper.Map<MstTestModel, MstTest>(mstTestModel);
                if (_mstTest.Id == 0)
                {
                    _context.MstTest.Add(_mstTest);
                }
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                _errorLog.BindErrorLogModel("InsertUpdateTest", ex.Message, "error");
                return -1;
            }
        }

        public int DeleteTest(int id)
        {
            try
            {
                // Check test have athletes or not
                int count = _context.TestAthleteMapping.Where(x => x.testId == id).Count();
                if (count > 0)
                {
                    //If test have athletes then remove before removing test
                    _context.TestAthleteMapping.RemoveRange(_context.TestAthleteMapping.Where(x => x.testId == id));
                    _context.SaveChanges();
                }

                MstTest _mstTest = _context.MstTest.Where(x => x.Id == id).FirstOrDefault();
                _context.MstTest.Remove(_mstTest);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                _errorLog.BindErrorLogModel("DeleteTest", ex.Message, "error");
                return -1;
            }
        }

        public List<MstUserModel> GetAthleteList()
        {
            try
            {
                return _context.MstUser.ProjectTo<MstUserModel>().Where(x => x.Role == Constants.role).ToList();
            }
            catch (Exception ex)
            {
                _errorLog.BindErrorLogModel("GetAthleteList", ex.Message, "error");
                return null;
            }
        }

        public List<TestAthleteMappingModel> GetTestDetailsWithId(int id)
        {
            try
            {
                return _context.TestAthleteMapping.Where(x => x.testId == id).Select(y => new TestAthleteMappingModel()
                {
                    AthleteId = y.AthleteId,
                    Id = y.Id,
                    AthleteName = _context.MstUser.Where(z => z.Id == y.AthleteId).Select(z => z.Name).FirstOrDefault(),
                    Distance = y.Distance,
                    FitnessRating = y.FitnessRating,
                    testId = y.testId
                }).ToList();
                //return _context.TestAthleteMapping.ProjectTo<TestAthleteMappingModel>().Where(x => x.testId == id).ToList();
            }
            catch (Exception ex)
            {
                _errorLog.BindErrorLogModel("GetTestDetailsWithId", ex.Message, "error");

                return null;
            }
        }

        public int InsertUpdateAthlete(TestAthleteMappingModel testAthleteMappingModel)
        {
            try
            {
                TestAthleteMapping testAthleteMapping = new TestAthleteMapping();
                testAthleteMapping = _context.TestAthleteMapping.Where(x => x.Id == testAthleteMappingModel.Id).FirstOrDefault();
                testAthleteMapping = _mapper.Map<TestAthleteMappingModel, TestAthleteMapping>(testAthleteMappingModel, testAthleteMapping);
                if (testAthleteMappingModel.Id == 0)
                {
                    _context.TestAthleteMapping.Add(testAthleteMapping);
                }
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                _errorLog.BindErrorLogModel("InsertUpdateAthlete", ex.Message, "error");

                return -1;
            }
        }

        public int DeleteAthlete(int id)
        {
            try
            {
                TestAthleteMapping _testAthleteMapping = _context.TestAthleteMapping.Where(x => x.Id == id).FirstOrDefault();
                _context.TestAthleteMapping.Remove(_testAthleteMapping);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                _errorLog.BindErrorLogModel("DeleteAthlete", ex.Message, "error");
                return -1;
            }
        }
    }
}
