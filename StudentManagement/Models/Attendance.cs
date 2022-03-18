using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Attendance
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string StudentId { get; set; }

        public User Student { get; set; }

        public Guid SubjectId { get; set; }

        public Subject Subject { get; set; }

        public DateTime PickedDate { get; set; }
    }
}
