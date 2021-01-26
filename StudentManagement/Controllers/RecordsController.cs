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
using StudentManagement.Models.RecordViewModel;
using StudentManagement.Service;

namespace StudentManagement.Controllers
{
    [Authorize(Roles = "Administrators, Teachers")]
    public class RecordsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public RecordsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

  
        // GET: Records
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = context.Records.Include(r => r.Creator).Include(r => r.Project);
            return View(await applicationDbContext.ToListAsync());
        }



        // GET: Records/Create
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

            var GetWeek = Week.Split(project.StartTime, project.DeadLine);


            this.ViewBag.Project = project;
            ViewBag.StudentName = new SelectList(context.Students, "StudentName", "StudentName");
            ViewBag.Week = new SelectList(GetWeek);
            return View(new RecordCreateViewModel());

        }

        // POST: Records/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? projectId, RecordCreateViewModel model)
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
                var record = new Record
                {
                    ProjectId = project.Id,
                    CreatorId = user.Id,

                    Created = now,
                    LogTime = model.LogTime,
                    StudentEmail = model.StudentName,
                    Week = model.Week
                };

                this.context.Add(record);
                await this.context.SaveChangesAsync();
                ViewBag.StudentName = new SelectList(context.Students, "StudentName", "StudentName");
                var GetWeek = Week.Split(project.StartTime, project.DeadLine);
                ViewBag.Week = new SelectList(GetWeek);
                return this.RedirectToAction("Details", "Projects", new { id = project.Id });
            }
            this.ViewBag.Project = project;
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await context.Records
                .Include(r => r.Creator)
                .Include(r => r.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            context.Records.Remove(record);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(Guid id)
        {
            return context.Records.Any(e => e.Id == id);
        }
    }
}
