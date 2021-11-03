using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.SubjectViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public AttendancesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? subjectId, SubjectDetailViewModel model)
        {
            if(subjectId == null)
            {
                return View("NotFound");
            }

            var subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == subjectId);

            if(subject == null)
            {
                return View("NotFound");
            }

            if (this.ModelState.IsValid)
            {
                var attendance = new Attendance
                {
                    SubjectId = subject.Id,
                    StudentId = model.AttendanceCreateViewModel.StudentId,
                    PickedDate = model.AttendanceCreateViewModel.PickedDate
                };

                this.context.Attendances.Add(attendance);

                await this.context.SaveChangesAsync();

                return RedirectToAction("Details", "Subjects", new { id = subject.Id });
            }

            return this.View(model);
        }
    }
}
