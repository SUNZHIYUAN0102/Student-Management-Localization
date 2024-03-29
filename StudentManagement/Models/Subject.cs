﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Subject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public User Creator { get; set; }
        public DateTime Created { get; set; }
        public string Code { get; set; }
        public string ThemeName { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<UserSubject> UserSubjects { get; set; }
    }
}
