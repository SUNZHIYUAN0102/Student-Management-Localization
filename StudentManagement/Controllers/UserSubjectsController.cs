using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Data;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using StudentManagement.Models.UserSubjectsViewModel;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class UserSubjectsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> usermanager;

        public UserSubjectsController(ApplicationDbContext context, UserManager<User> usermanager)
        {
            this.context = context;
            this.usermanager = usermanager;
        }

        [HttpGet]
        public async Task<IActionResult> MySubject()
        {
            var user = await this.usermanager.GetUserAsync(this.HttpContext.User);
            var mySubjects = this.context.UserSubjects
                .Include(x => x.Subject).ThenInclude(x => x.Creator)
                .Include(x => x.Subject.Projects)
                .Where(x => x.UserId == user.Id)
                .AsEnumerable();
            return View(new MySubjectsViewModel { UserSubjects = mySubjects});
        }

        [Authorize(Roles = "Administrator, Teacher, Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(MySubjectsViewModel model)
        {
            var subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Code == model.JoinSubjectViewModel.Code);

            if(subject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var user = await this.usermanager.GetUserAsync(this.HttpContext.User);

            var roles = HttpContext.User.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value)
                    .ToArray();
            
            var isTeacher = roles.Any(x => x == "Administrator" || x == "Teacher" );

            if (this.ModelState.IsValid)
            {
                var userSubject = new UserSubject
                {
                    SubjectId = subject.Id,
                    UserId = user.Id,
                    Role = isTeacher ? "Teacher" : "Student"
                };
                this.context.UserSubjects.Add(userSubject);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction(nameof(MySubject));
            }
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(Guid? subjectId)
        {
            if (subjectId == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == subjectId);

            if (subject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var user = await this.usermanager.GetUserAsync(this.HttpContext.User);

            var userSubject = await this.context.UserSubjects.SingleOrDefaultAsync(x => x.SubjectId == subject.Id && x.UserId == user.Id);

            if(userSubject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            this.context.UserSubjects.Remove(userSubject);
            await this.context.SaveChangesAsync();
            return RedirectToAction(nameof(MySubject));
        }
    }
}
