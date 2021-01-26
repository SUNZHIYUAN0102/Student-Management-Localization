using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.NoteViewModel
{
    public class NoteEditViewModel
    {
        [Required]
        public String Text { get; set; }
    }
}
