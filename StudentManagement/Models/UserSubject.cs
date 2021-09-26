using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class UserSubject
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public Guid SubjectId { get; set; }

        public Subject Subject { get; set; }
    }
}
