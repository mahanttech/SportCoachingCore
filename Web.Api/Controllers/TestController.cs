using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Infrastructure.Interface;
using Web.Api.Infrastructure.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Controllers
{
   // [Authorize]
    [Route("api/test")]
    [ApiController]
    public class SportTestController : Controller
    {

        private readonly ITestRepository _iTest;
        public SportTestController(ITestRepository iTest)
        {
            _iTest = iTest;
        }


        // GET: api/<controller>
        /// <summary>
        /// Method will return test list
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("TestList")]
        [HttpGet]
        public IActionResult GetTestList()
        {
            return Ok(_iTest.GetTestList());
        }

        // GET: api/<controller>
        /// <summary>
        /// Method will return test detail with athlete test results
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("TestDetails")]
        [HttpGet]
        public IActionResult GetTestDetailsWithId(int id)
        {
            if (id == 0)
            {
                return BadRequest("Please provide id");
            }
            return Ok(_iTest.GetTestDetailsWithId(id));
        }

        // POst: api/<controller>
        /// <summary>
        /// Method will take MstTestModel as parameter
        /// Method will insert and update test
        /// </summary>
        /// <returns>Method will return 1 for success,-1 for error</returns>
        /// 
        [Route("AddUpdateTest")]
        [HttpPost]
        public IActionResult InsertUpdateTest(MstTestModel mstTestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_iTest.InsertUpdateTest(mstTestModel));
        }

        /// <summary>
        /// Method will remove test from db by id
        /// </summary>
        /// <param name="id">Test id</param>
        /// <returns>return 1 for success and -1 for error</returns>
        [Route("DeleteTest")]
        [HttpGet]
        public IActionResult DeleteTest(int id)
        {
            if (id == 0)
            {
                return BadRequest("Please provide id");
            }
            return Ok(_iTest.DeleteTest(id));
        }

        // GET: api/<controller>
        /// <summary>
        /// Method will return athlete list
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("AthleteList")]
        [HttpGet]
        public IActionResult GetAthleteList()
        {
            return Ok(_iTest.GetAthleteList());
        }

        // POst: api/<controller>
        /// <summary>
        /// Method will take TestAthleteMappingModel as parameter
        /// Method will insert and update athlete for test
        /// </summary>
        /// <returns>Method will return 1 for success,-1 for error</returns>
        /// 
        [Route("AddUpdateAthlete")]
        [HttpPost]
        public IActionResult InsertUpdateAthlete(TestAthleteMappingModel testAthleteMappingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_iTest.InsertUpdateAthlete(testAthleteMappingModel));
        }

        /// <summary>
        /// Method will remove test from db by id
        /// </summary>
        /// <param name="id">Athlete transaction id</param>
        /// <returns>return 1 for success and -1 for error</returns>
        [Route("DeleteAthlete")]
        [HttpGet]
        public IActionResult DeleteAthlete(int id)
        {
            if (id == 0)
            {
                return BadRequest("Please provide id");
            }
            return Ok(_iTest.DeleteAthlete(id));
        }


    }
}
