using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.SubjectViewModel
{
    public class SubjectDetailViewModel
    {
        public Subject Subject { get; set; }

        public NotificationViewModel.NotificationCreateViewModel NotificationCreateViewModel { get; set; }
    }
}
