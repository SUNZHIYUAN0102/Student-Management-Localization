using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.AttendanceViewModel
{
    public class AttendanceCreateViewModel
    {
        [Required]
        public DateTime PickedDate { get; set; } = DateTime.Now;

        [Required]
        public string StudentId { get; set; }
    }
}
