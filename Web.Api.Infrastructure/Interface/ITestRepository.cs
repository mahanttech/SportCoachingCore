using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Models;

namespace Web.Api.Infrastructure.Interface
{
    public interface ITestRepository
    {
        List<MstTestModel> GetTestList(); //Method to get all test as list
        List<TestAthleteMappingModel> GetTestDetailsWithId(int id); // Method to get test by id
        int InsertUpdateTest(MstTestModel mstTestModel); // Method to Insert/Update test
        int DeleteTest(int id); // Method to delete test and its transaction with athleteMapping table

        List<MstUserModel> GetAthleteList(); // Method to get all athlete for bind ddl
        int InsertUpdateAthlete(TestAthleteMappingModel testAthleteMappingModel); // Method to Insert/update athlete for test
        int DeleteAthlete(int id); // Method to delete athlete
    }
}
