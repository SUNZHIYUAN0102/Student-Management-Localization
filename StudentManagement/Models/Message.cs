using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public User FromUser { get; set; }
        public Guid ToRoomId { get; set; }
        public Room ToRoom { get; set; }
    }
}
