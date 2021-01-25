using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw_4.DTOs.Request
{
    public class EnrollStudentRequest
    {
        
            [Required]
            [MaxLength(100)]
            public string IndexNumber { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public DateTime BirthDate { get; set; }
            [Required]
            public string Name { get; set; } 
    
    }
}
