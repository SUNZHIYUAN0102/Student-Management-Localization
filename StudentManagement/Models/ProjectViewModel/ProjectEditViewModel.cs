using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.ProjectViewModel
{
    public class ProjectEditViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime DeadLine { get; set; }

        [Required]
        public Boolean IsPrivate { get; set; }
    }
}
