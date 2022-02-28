using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Attachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FileName { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime Created { get; set; }

        public int Score { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

    }
}
