﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.RecordViewModel;
using StudentManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class RecordsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public RecordsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? projectId)
        {
            if(projectId == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var project = await this.context.Projects
                .Include(x=>x.Subject).ThenInclude(x=>x.UserSubjects)
                .ThenInclude(x=>x.User)
                .SingleOrDefaultAsync(x => x.Id == projectId);

            if(project == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var getWeek = Week.Split(project.StartTime, project.DeadLine);

            var students = project.Subject.UserSubjects.Where(x=>x.Role=="Student").Select(x=>x.User);
 
            ViewBag.Students = new SelectList(students, "Id", "FullName");

            ViewBag.Week = new SelectList(getWeek);

            ViewBag.projectId = project.Id;

            return View(new RecordCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? projectId, RecordCreateViewModel model)
        {
            if (projectId == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var project = await this.context.Projects
               .Include(x => x.Subject).ThenInclude(x => x.UserSubjects)
               .ThenInclude(x => x.User)
               .SingleOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var getWeek = Week.Split(project.StartTime, project.DeadLine);

            var students = project.Subject.UserSubjects.Where(x => x.Role == "Student").Select(x => x.User);

            ViewBag.Students = new SelectList(students, "Id", "FullName");

            ViewBag.Week = new SelectList(getWeek);

            ViewBag.projectId = project.Id;

            var now = DateTime.Now;

            if (this.ModelState.IsValid)
            {
                var record = new Record()
                {
                    StudentId = model.StudentId,
                    LogTime = model.LogTime,
                    Week = model.Week,
                    ProjectId = project.Id,
                    Created = now
                };

                this.context.Records.Add(record);
                await this.context.SaveChangesAsync();
                return RedirectToAction("Details", "Projects", new { id = project.Id });
            }
            return this.View(model);
        }

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var record = await this.context.Records
                .Include(x=>x.Project).ThenInclude(x=>x.Subject).ThenInclude(x=>x.UserSubjects).ThenInclude(x=>x.User)
                .SingleOrDefaultAsync(x => x.Id == id);


            if(record == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var getWeek = Week.Split(record.Project.StartTime, record.Project.DeadLine);

            var students = record.Project.Subject.UserSubjects.Where(x => x.Role == "Student").Select(x => x.User);

            ViewBag.Students = new SelectList(students, "Id", "FullName");

            ViewBag.Week = new SelectList(getWeek);

            var model = new RecordEditViewModel
            {
                StudentId = record.StudentId,
                LogTime = record.LogTime,
                Week = record.Week
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, RecordEditViewModel model)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var record = await this.context.Records
                .Include(x => x.Project).ThenInclude(x => x.Subject).ThenInclude(x => x.UserSubjects).ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);


            if (record == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var getWeek = Week.Split(record.Project.StartTime, record.Project.DeadLine);

            var students = record.Project.Subject.UserSubjects.Where(x => x.Role == "Student").Select(x => x.User);

            ViewBag.Students = new SelectList(students, "Id", "FullName");

            ViewBag.Week = new SelectList(getWeek);

            if (this.ModelState.IsValid)
            {
                record.StudentId = model.StudentId;
                record.LogTime = model.LogTime;
                record.Week = model.Week;
                await this.context.SaveChangesAsync();

                return RedirectToAction("Details", "Projects", new { id = record.ProjectId });
            }

            return this.View(model);
        }
    }

}
