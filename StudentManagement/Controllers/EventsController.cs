using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudentManagement.Data;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly UserManager<User> userManager;

        public EventsController(ApplicationDbContext dbcontext, UserManager<User> userManager)
        {
            this.dbcontext = dbcontext;
            this.userManager = userManager;
        }

        [HttpGet("my-events")]
        public IActionResult MyEvent()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<JsonResult> GetEvents()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var myEvent = this.dbcontext.Events.Include(x => x.User).Where(x => x.User == user);
            return Json(new { data = myEvent });
        }

        [HttpDelete]
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
        public async Task<JsonResult> SaveEvent(CreateEventModel e)
        {
            var status = false;

            //if (e.Id != null)
            //{
            //    var currEvent = await this.dbcontext.Events.Where(x => x.Id == e.Id).SingleOrDefaultAsync();

            //    if (currEvent != null)
            //    {
            //        currEvent.Title = e.Title;
            //        currEvent.Description = e.Description;
            //        currEvent.StartTime = e.StartTime;
            //        currEvent.EndTime = e.EndTime;
            //        currEvent.ThemeColor = e.ThemeColor;
            //    }
            //}
            //else
            //{
               
            //}

            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var newEvent = new Event
            {
                Title = e.Title,
                Description = e.Description,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                ThemeColor = e.ThemeColor,
                User = user
            };
            this.dbcontext.Events.Add(newEvent);

            await this.dbcontext.SaveChangesAsync();
            status = true;

            return Json(new { Data = new { status = status } });
        }
    }
}
