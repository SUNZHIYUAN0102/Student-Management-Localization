using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.UserSubjectsViewModel
{
    public class MySubjectsViewModel
    {
        public IEnumerable<UserSubject> UserSubjects { get; set; }

        public JoinSubjectViewModel JoinSubjectViewModel { get; set; }
    }
}
