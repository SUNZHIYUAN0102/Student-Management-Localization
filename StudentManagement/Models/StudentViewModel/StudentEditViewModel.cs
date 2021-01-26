using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.StudentViewModel
{
    public class StudentEditViewModel
    {
        [Required]
        public string StudentName { get; set; }

        [Required]
        public string GroupNumber { get; set; }
    }
}
