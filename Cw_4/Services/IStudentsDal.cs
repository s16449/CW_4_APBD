using Cw_4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw_4.Services
{
    public interface IStudentsDal
    {
        public IEnumerable<Student> GetStudents();
        public IEnumerable<Enrollment> GetEnrollment(string id);
    }
}
