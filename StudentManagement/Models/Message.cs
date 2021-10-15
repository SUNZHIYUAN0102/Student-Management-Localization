using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Text { get; set; }
        public DateTime Created { get; set; }

        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }
    }
}
