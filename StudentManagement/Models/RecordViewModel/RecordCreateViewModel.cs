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
        [Required]
        public float LogTime { get; set; }
        [Required]
        public string Week { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public int Progress { get; set; }   
    }
}
