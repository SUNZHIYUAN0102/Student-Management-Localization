using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.RecordViewModel
{
    public class RecordCreateViewModel
    {
        [Required]
        public string StudentId { get; set; }

        public float LogTime { get; set; }

        public string Week { get; set; }

    }
}
