﻿using System;
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
using System.ComponentModel;

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

        public async Task<IActionResult> Index()
        {
            var projects = context.Projects
                .Include(p => p.Notes)
                .Include(p => p.Creator);

            return View(await projects.ToListAsync());
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var project = await context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Notes)
                .Include(p => p.Records)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return this.NotFound();
            }

            return View(project);
        }

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? subjectId)
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
            this.ViewBag.Subject = subject;
            return View(new ProjectCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? subjectId, ProjectCreateViewModel model)
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

            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
           
            if (this.ModelState.IsValid)
            {
                var now = DateTime.Now;
                var project = new Project
                {
                    SubjectId = subject.Id,
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
            this.ViewBag.Subject = subject;
            return this.View(model);
        }

        [Authorize(Roles = "Administrator, Teacher")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var project = await context.Projects.FindAsync(id);

            if (project == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
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
        public async Task<IActionResult> Edit(Guid? id, ProjectEditViewModel model)
        {
            if (id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var project = await this.context.Projects.SingleOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (this.ModelState.IsValid)
            {
                project.Title = model.Title;
                project.Description = model.Description;
                project.StartTime = model.StartTime;
                project.DeadLine = model.DeadLine;
                project.IsPrivate = model.IsPrivate;
                project.Modified = DateTime.Now;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
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

            var project = await this.context.Projects.SingleOrDefaultAsync(x => x.Id == id);

            if (project == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            try
            {
                this.context.Projects.Remove(project);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.ErrorTitle = "Project can't be deleted";
                ViewBag.ErrorMessage = "Since there are records or notes in this subjects";
                return View("~/Views/Shared/DeleteError.cshtml");
            }
        }
    }

}