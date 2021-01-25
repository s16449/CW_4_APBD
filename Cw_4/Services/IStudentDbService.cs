using Cw_4.DTOs.Request;
using Cw_4.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw_4.Services
{
    public interface IStudentDbService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        StudentPromotionResponse StudentPromotion(StudentPromotionRequest request);
    }
}
