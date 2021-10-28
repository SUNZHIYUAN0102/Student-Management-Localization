using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }

        public DateTime Registered { get; set; }

        public string Degree { get; set; }

        public string Exp { get; set; }

        public string Address { get; set; }

        public string FaceBookUrl { get; set; }

        public string Phone { get; set; }

        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }

        public string VKUrl { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public ICollection<Note> Notes { get; set; }

        public ICollection<UserSubject> UserSubjects { get; set; }
        public ICollection<Project> Projects { get; set; }

        public ICollection<Record> Records { get; set; }

        public ICollection<Record> CreatedRecords { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<Message> Messages { get; set; }
    }

}
