using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.EventViewModel
{
    public class EventCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndTime { get; set; } = DateTime.Now;

        public string ThemeColor { get; set; }
    }
}
