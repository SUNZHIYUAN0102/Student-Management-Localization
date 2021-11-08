using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public String CreatorId { get; set; }

        public User Creator { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime DeadLine { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public Boolean IsPrivate { get; set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<Note> Notes { get; set; }

        public ICollection<Record> Records { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}
