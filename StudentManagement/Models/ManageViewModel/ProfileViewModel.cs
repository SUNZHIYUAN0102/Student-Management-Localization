using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.ManageViewModel
{
    public class ProfileViewModel
    {
        public ChangePasswordViewModel changePasswordViewModel { get; set; }

        public ChangeUserNameViewModel changeUserNameViewModel { get; set; }

        public ChangeEmailViewModel changeEmailViewModel { get; set; }

        public User user { get; set; }
    }
}
