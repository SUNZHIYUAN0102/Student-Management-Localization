using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.ProjectViewModel
{
    public class ProjectCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;
        [Required]
        public DateTime DeadLine { get; set; } = DateTime.Now;

        [Required]
        public Boolean IsPrivate { get; set; }
    }
}
