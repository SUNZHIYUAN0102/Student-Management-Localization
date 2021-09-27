using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.SubjectViewModel
{
    public class SubjectEditViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
