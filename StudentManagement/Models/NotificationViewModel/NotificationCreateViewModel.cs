using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.NotificationViewModel
{
    public class NotificationCreateViewModel
    {
        [Required]
        public string Text { get; set; }
    }
}
