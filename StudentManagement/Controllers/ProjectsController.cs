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
using StudentManagement.Models.ProjectViewModel;
using StudentManagement.Service;
using PagedList.Mvc;
using PagedList;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;


        public ProjectsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index([FromQuery]string search, [FromQuery]int pageNumber = 1)
        {
            var projects = context.Projects
                .Include(p => p.Notes)
                .Include(p => p.Creator).AsQueryable();

            if(search != null)
            {
                projects = projects.Where(p => p.Title.Contains(search));
            }

            ViewBag.Search = search;
            return View("Index", await PaginatedList<Project>.CreateAsync(projects, pageNumber, 5));
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var project = await context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Notes)
                .Include(p => p.Records)
                .ThenInclude(p => p.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return this.NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Administrators, Teachers")]
        public IActionResult Create()
        {
            return View(new ProjectEditViewModel());
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectEditViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var project = new Project
                {
                    CreatorId = user.Id,
                    Created = now,
                    Modified = now,
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    DeadLine = model.DeadLine,
                    IsPrivate = model.IsPrivate
                };

                this.context.Projects.Add(project);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Administrators, Teachers")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var model = new ProjectEditViewModel
            {
                Title = project.Title,
                Description = project.Description,
                StartTime = project.StartTime,
                DeadLine = project.DeadLine,
                IsPrivate = project.IsPrivate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProjectEditViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var project = await this.context.Projects.SingleOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return this.NotFound();
            }
            if (this.ModelState.IsValid)
            {
                project.Title = model.Title;
                project.Description = model.Description;
                project.StartTime = model.StartTime;
                project.DeadLine = model.DeadLine;
                project.IsPrivate = model.IsPrivate;
                project.Modified = DateTime.UtcNow;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            return this.View(model);
        }

 
        [Authorize(Roles = "Administrators, Teachers")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var project = await this.context.Projects.FindAsync(id);
                context.Projects.Remove(project);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ViewBag.ErrorTitle = "Project can't be deleted";
                ViewBag.ErrorMessage = "Project can't be deleted as there are comments or records in this project. If you want to delete user please remove comments or records from project first";
                return this.View("DeleteError");
            }
        }

        private bool ProjectExists(Guid id)
        {
            return context.Projects.Any(e => e.Id == id);
        }
    }
}