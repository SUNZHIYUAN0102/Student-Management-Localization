﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.AccountViewModel
{
    public class AddPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage ="The new password and confirmation password don't match.")]
        public string ConfirmPassword { get; set; }
    }
}
