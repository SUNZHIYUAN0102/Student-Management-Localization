using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Record
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public String CreatorId { get; set; }

        public User Creator { get; set; }

        public string StudentEmail { get; set; }

        public DateTime Created { get; set; }

        public String Week { get; set; }

        public float LogTime { get; set; }
    }
}
