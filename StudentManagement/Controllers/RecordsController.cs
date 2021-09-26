//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using StudentManagement.Data;
//using StudentManagement.Models;
//using StudentManagement.Models.RecordViewModel;
//using StudentManagement.Service;

//namespace StudentManagement.Controllers
//{
//    [Authorize(Roles = "Administrator, Teacher")]
//    public class RecordsController : Controller
//    {
//        private readonly ApplicationDbContext context;
//        private readonly UserManager<User> userManager;

//        public RecordsController(ApplicationDbContext context, UserManager<User> userManager)
//        {
//            this.context = context;
//            this.userManager = userManager;
//        }

  
//        // GET: Records
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = context.Records.Include(r => r.Creator).Include(r => r.Project);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Records/Create
//        public async Task<IActionResult> Create(Guid? projectId)
//        {
//            if (projectId == null)
//            {
//                return this.NotFound();
//            }
//            var project = await this.context.Projects
//                .SingleOrDefaultAsync(m => m.Id == projectId);

//            if (project == null)
//            {
//                return this.NotFound();
//            }
//            var GetWeek = Week.Split(project.StartTime, project.DeadLine);    
//            ViewBag.StudentName = new SelectList(context.Students, "StudentName", "StudentName");
//            this.ViewBag.Project = project;
//            ViewBag.Week = new SelectList(GetWeek);
//            return View(new RecordCreateViewModel());
//        }

//        // POST: Records/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Guid? projectId, RecordCreateViewModel model)
//        {
//            if (projectId == null)
//            {
//                return this.NotFound();
//            }
//            var project = await this.context.Projects
//                .SingleOrDefaultAsync(m => m.Id == projectId);
//            if (project == null)
//            {
//                return this.NotFound();
//            }
//            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

//            if (this.ModelState.IsValid)
//            {
//                var now = DateTime.UtcNow;
//                var record = new Record
//                {
//                    ProjectId = project.Id,
//                    CreatorId = user.Id,
//                    Created = now,
//                    LogTime = model.LogTime,
//                    StudentEmail = model.StudentName,
//                    Week = model.Week
//                };
//                this.context.Add(record);
//                await this.context.SaveChangesAsync();
//                ViewBag.StudentName = new SelectList(context.Students, "StudentName", "StudentName");
//                var GetWeek = Week.Split(project.StartTime, project.DeadLine);
//                ViewBag.Week = new SelectList(GetWeek);
//                return this.RedirectToAction("Details", "Projects", new { id = project.Id });
//            }
//            this.ViewBag.Project = project;
//            return View(model);
//        }

//        public async Task<IActionResult> Edit(Guid? id)
//        {
//            if(id == null)
//            {
//                return this.NotFound();
//            }

//            var record =await context.Records
//                .Include(x => x.Project)
//                .SingleOrDefaultAsync(x => x.Id == id);
             
//            var project = record.Project;

//            var model = new RecordCreateViewModel
//            {
//                StudentName = record.StudentEmail,
//                LogTime = record.LogTime,
//                Week = record.Week
//            };

//            ViewBag.StudentName = new SelectList(context.Students, "StudentName", "StudentName");

//            var GetWeek = Week.Split(project.StartTime, project.DeadLine);
//            ViewBag.Id = record.Id;
//            ViewBag.Week = new SelectList(GetWeek);
//            return View(model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(Guid? id, RecordCreateViewModel model)
//        {
//            var record = await this.context.Records.FindAsync(id);
//            var project = record.Project;

//            if(this.ModelState.IsValid)
//            {
//                record.StudentEmail = model.StudentName;
//                record.LogTime = model.LogTime;
//                record.Week = model.Week;
//                await this.context.SaveChangesAsync();
//                return RedirectToAction("Details", "Projects", new { id = record.ProjectId });
//            }


//            ViewBag.StudentName = new SelectList(context.Students, "StudentName", "StudentName");

//            var GetWeek = Week.Split(project.StartTime, project.DeadLine);
//            ViewBag.Week = new SelectList(GetWeek);
//            return View(model);
//        }


//        public async Task<IActionResult> Delete(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var record = await context.Records
//                .Include(r => r.Creator)
//                .Include(r => r.Project)
//                .FirstOrDefaultAsync(m => m.Id == id);

//            if (record == null)
//            {
//                return NotFound();
//            }
//            context.Records.Remove(record);
//            await context.SaveChangesAsync();
//            return RedirectToAction("Details", "Projects", new { id = record.ProjectId});
//        }

//        public async Task<IActionResult> TableDelete(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var record = await context.Records
//                .Include(r => r.Creator)
//                .Include(r => r.Project)
//                .FirstOrDefaultAsync(m => m.Id == id);

//            if (record == null)
//            {
//                return NotFound();
//            }
//            context.Records.Remove(record);
//            await context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool RecordExists(Guid id)
//        {
//            return context.Records.Any(e => e.Id == id);
//        }
//    }
//}
