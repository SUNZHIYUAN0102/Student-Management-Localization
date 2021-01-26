using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string StudentName { get; set; }

        public string GroupNumber { get; set; }

    }

}



