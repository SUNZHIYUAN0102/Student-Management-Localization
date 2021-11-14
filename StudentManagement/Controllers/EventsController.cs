﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.EventViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly UserManager<User> userManager;

        public EventsController(ApplicationDbContext dbcontext, UserManager<User> userManager)
        {
            this.dbcontext = dbcontext;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> MyEvent()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var myEvent = await this.dbcontext.Events.Include(x => x.User).Where(x => x.User == user).ToListAsync();
            return this.View(new EventDetailViewModel { Events = myEvent });
        }

        [HttpGet]
        public async Task<JsonResult> GetEvents()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var myEvent = this.dbcontext.Events.Include(x => x.User).Where(x => x.User == user);
            return Json(new { data = myEvent });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteEvent(Guid eventId)
        {
            var status = false;
            var v = await this.dbcontext.Events.Where(x => x.Id == eventId).SingleOrDefaultAsync();
            if (v != null)
            {
                this.dbcontext.Remove(v);
                await this.dbcontext.SaveChangesAsync();
                status = true;
            }
            return Json(new { Data = new { status = status } });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventDetailViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(this.HttpContext.User);

                var myEvent = new Event
                {
                    Title = model.EventCreateViewModel.Title,
                    Description = model.EventCreateViewModel.Description,
                    StartTime = model.EventCreateViewModel.StartTime,
                    EndTime = model.EventCreateViewModel.EndTime,
                    UserId = user.Id,
                    ThemeColor = model.EventCreateViewModel.ThemeColor
                };

                this.dbcontext.Events.Add(myEvent);
                await this.dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(MyEvent));
            }

            return this.View(model);
        }
    }
}