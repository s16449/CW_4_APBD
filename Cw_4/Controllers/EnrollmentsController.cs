using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw_4.DTOs.Request;
using Cw_4.Models;
using Cw_4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw_4.Controllers
{
    [ApiController]

    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/enrollments")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var response = _service.EnrollStudent(request);

            if (response.answer == "OK")
            {
                return Created(response.answer, response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("api/enrollments/promotions")]
        public IActionResult StudentPromotion(StudentPromotionRequest request)
        {
            var response = _service.StudentPromotion(request);

            if (response.answer == "OK")
            {
                return Created(response.answer, response);
            }
            else
            {
                return NotFound(response);
            }
        }
    }
}
