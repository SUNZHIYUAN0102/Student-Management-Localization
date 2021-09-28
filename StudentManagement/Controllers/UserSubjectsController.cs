﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Data;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
                .Include(x=>x.Subject).ThenInclude(x=>x.Creator)
                .Where(x => x.UserId == user.Id);
            return this.View(await mySubjects.ToListAsync());
        }

        [Authorize(Roles = "Administrator, Teacher, Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(Guid? subjectId)
        {
            if(subjectId == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == subjectId);

            if(subject == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var user = await this.usermanager.GetUserAsync(this.HttpContext.User);

            if(this.ModelState.IsValid)
            {
                var userSubject = new UserSubject
                {
                    SubjectId = subject.Id,
                    UserId = user.Id
                };
                this.context.UserSubjects.Add(userSubject);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction(nameof(MySubject));
            }
            return this.View();
        }

        [Authorize(Roles = "Administrator, Teacher")]
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
