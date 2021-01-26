using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
        
        public String CreatorId { get; set; }

        public User Creator { get; set; }

        public DateTime Created { get; set; }

        public String Text { get; set; }
    }
}
