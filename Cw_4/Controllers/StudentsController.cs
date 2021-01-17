using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw_4.Models;
using Cw_4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw_4.Controllers
    
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
 {
        private IStudentsDal _dbService;


        public StudentsController(IStudentsDal dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudent()
        {
            return Ok(_dbService.GetStudents());
        }


        [HttpGet("{id}")]
        public IActionResult GetEnrollment(string id)
        {
            return Ok(_dbService.GetEnrollment(id));
        }

    }
}
