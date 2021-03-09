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

        [Authorize(Roles = "Administrators")]
        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = context.Notes
                .Include(n => n.Creator)
                .Include(n => n.Project);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Notes/Create
        public async Task<IActionResult> Create(Guid? projectId)
        {
            if (projectId == null)
            {
                return this.NotFound();
            }
            var project = await this.context.Projects
                .SingleOrDefaultAsync(m => m.Id == projectId);
            if (project == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Project = project;
            return View(new NoteEditViewModel());
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? projectId, NoteEditViewModel model)
        {
            if (projectId == null)
            {
                return this.NotFound();
            }
            var project = await this.context.Projects
                .SingleOrDefaultAsync(m => m.Id == projectId);

            if (project == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (this.ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var note = new Note
                {
                    ProjectId = project.Id,
                    CreatorId = user.Id,

                    Created = now,
                    Text = model.Text
                };

                this.context.Add(note);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Details", "Projects", new { id = project.Id });
            }
            this.ViewBag.Project = project;
            return View(model);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var note = await this.context.Notes
                .SingleOrDefaultAsync(m => m.Id == id);

            if (note == null || !this.userPermission.CanEditNote(note))
            {
                return this.NotFound();
            }

            var model = new NoteEditViewModel
            {
                Text = note.Text
            };

            this.ViewBag.Project = note.Project;
            return this.View(model);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, NoteEditViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var note = await this.context.Notes
                .SingleOrDefaultAsync(m => m.Id == id);

            if (note == null || !this.userPermission.CanEditNote(note))
            {
                return this.NotFound();
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
                return this.NotFound();
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


        public async Task<IActionResult> TableDelete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
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
            return RedirectToAction(nameof(Index));
        }


        private bool NoteExists(Guid id)
        {
            return context.Notes.Any(e => e.Id == id);
        }
    }
}