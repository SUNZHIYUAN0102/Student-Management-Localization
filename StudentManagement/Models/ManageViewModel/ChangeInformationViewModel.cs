using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.ManageViewModel
{
    public class ChangeInformationViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Degree { get; set; }

        public string Exp { get; set; }

        public string Address { get; set; }

        public string FaceBookUrl { get; set; }

        public string Phone { get; set; }

        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }

        public string VKUrl { get; set; }
    }
}
