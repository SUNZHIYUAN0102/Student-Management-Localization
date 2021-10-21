using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.UserSubjectsViewModel
{
    public class JoinSubjectViewModel
    {
        [Required]
        public string Code { get; set; }
    }
}
