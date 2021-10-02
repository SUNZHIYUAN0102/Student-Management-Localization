using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.ProjectViewModel
{
    public class ProjectDetailViewModel
    {
        public Project Project { get; set; }

        public NoteViewModel.NoteEditViewModel NoteEditViewModel { get; set; }
    }
}
