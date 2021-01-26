using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImagePath { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public ICollection<Project> Projects { get; set; }
        public ICollection<Note> Notes { get; set; }

        public ICollection<Record> Records { get; set; }
    }

}
