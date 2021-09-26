using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImagePath { get; set; }

        public DateTime Registered { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public ICollection<Note> Notes { get; set; }

        public ICollection<UserSubject> UserSubjects { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }

        public ICollection<Record> Records { get; set; }
    }

}
