using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.RecordViewModel
{
    public class RecordEditViewModel
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public float LogTime { get; set; }
        [Required]
        public string Week { get; set; }
    }
}
