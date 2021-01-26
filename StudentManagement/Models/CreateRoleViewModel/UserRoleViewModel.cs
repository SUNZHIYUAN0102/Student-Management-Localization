using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.CreateRoleViewModel
{
    public class UserRoleViewModel
    {
     
        public string UserId { get; set; }
        public string Email { get; set; }

        public bool IsSelected { get; set; }
    }
}
