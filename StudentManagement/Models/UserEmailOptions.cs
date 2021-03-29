using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class UserEmailOptions
    {
        public List<string> ToEmails { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
