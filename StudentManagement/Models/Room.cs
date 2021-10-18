﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Room
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
