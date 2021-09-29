using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public SubjectsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
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
                .Include(x=>x.Projects)
                .Include(x => x.UserSubjects).ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return this.View(subject);
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
                    Created = now
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
                Name = subject.Name
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
    }
}
