﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.CreateRoleViewModel
{
    public class EditRolesViewModel
    {
        public EditRolesViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }

        [Required(ErrorMessage ="Role Name Is Required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
