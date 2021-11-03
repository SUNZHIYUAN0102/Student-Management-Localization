using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.SubjectViewModel;
using StudentManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public SubjectsController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var subject = this.context.Subjects
                .Include(x => x.Creator);

            return View(await subject.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var subject = await this.context.Subjects
                .Include(x => x.Projects)
                .Include(x => x.UserSubjects)
                .ThenInclude(x => x.User)
                .Include(x => x.Notifications)
                .Include(x => x.Attendances).ThenInclude(x => x.Student)
                .SingleOrDefaultAsync(x => x.Id == id);

            ViewBag.users = subject.UserSubjects.GroupBy(x => x.Role);

            var students = subject.UserSubjects.Where(x => x.Role == "Student").Select(x => x.User);

            ViewBag.Students = new SelectList(students, "Id", "FullName");

            if (subject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(new SubjectDetailViewModel { Subject = subject });
        }

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View(new SubjectCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectCreateViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (this.ModelState.IsValid)
            {
                var now = DateTime.Now;
                var subject = new Subject
                {
                    Name = model.Name,
                    Creator = user,
                    Created = now,
                    ThemeName = model.ThemeName,
                    Code = RandomAlphanumeric.RandomCode()
                };
                this.context.Subjects.Add(subject);
                await this.context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return this.View(model);
        }

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if(id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == id);

            if(subject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var model = new SubjectEditViewModel()
            {
                Name = subject.Name,
                ThemeName = subject.ThemeName
            };
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, SubjectEditViewModel model)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (this.ModelState.IsValid)
            {
                subject.Name = model.Name;
                subject.ThemeName = model.ThemeName;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(model);
        }

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            try
            {
                this.context.Subjects.Remove(subject);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorTitle = "Subject can't be deleted";
                ViewBag.ErrorMessage = "Since there are projects in this subjects";
                return View("~/Views/Shared/DeleteError.cshtml");
            }
        }

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public async Task<IActionResult> ResetCode(Guid? subjectId)
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

            if (this.ModelState.IsValid)
            {
                subject.Code = RandomAlphanumeric.RandomCode();
                await this.context.SaveChangesAsync();
                return RedirectToAction("Details", "Subjects", new { id = subject.Id });
            }

            return this.View();
        }


        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public async Task<IActionResult> RemoveCode(Guid? subjectId)
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

            if (this.ModelState.IsValid)
            {
                subject.Code = null;
                await this.context.SaveChangesAsync();
                return RedirectToAction("Details", "Subjects", new { id = subject.Id });
            }

            return this.View();
        }
    }
}
