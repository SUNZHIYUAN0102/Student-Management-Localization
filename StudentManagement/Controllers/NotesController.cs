using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.NoteViewModel;
using StudentManagement.Models.ProjectViewModel;
using StudentManagement.Service;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IUserPermission userPermission;

        public NotesController(ApplicationDbContext context, UserManager<User> userManager, IUserPermission userPermission)
        {
            this.context = context;
            this.userPermission = userPermission;
            this.userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? projectId, ProjectDetailViewModel model)
        {
            if (projectId == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var project = await this.context.Projects
                .SingleOrDefaultAsync(m => m.Id == projectId);

            if (project == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (this.ModelState.IsValid)
            {
                var now = DateTime.Now;
                var note = new Note
                {
                    ProjectId = project.Id,
                    CreatorId = user.Id,

                    Created = now,
                    Text = model.NoteEditViewModel.Text
                };

                this.context.Add(note);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Details", "Projects", new { id = project.Id });
            }
            return this.View();
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var note = await this.context.Notes
                .SingleOrDefaultAsync(m => m.Id == id);

            if (note == null || !this.userPermission.CanEditNote(note))
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var model = new NoteEditViewModel
            {
                Text = note.Text
            };

            this.ViewBag.Project = note.Project;
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, NoteEditViewModel model)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var note = await this.context.Notes
                .SingleOrDefaultAsync(m => m.Id == id);

            if (note == null || !this.userPermission.CanEditNote(note))
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                note.Text = model.Text;
                await this.context.SaveChangesAsync();
                return RedirectToAction("Details", "Projects", new { id = note.ProjectId });
            }

            this.ViewBag.Project = note.Project;
            return this.View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var note = await this.context.Notes
                .Include(n => n.Creator)
                .Include(n => n.Project)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (note == null || !this.userPermission.CanEditNote(note))
            {
                return NotFound();
            }

            this.context.Notes.Remove(note);
            await context.SaveChangesAsync();
            return RedirectToAction("Details", "Projects", new { id = note.ProjectId });
        }
    }
}