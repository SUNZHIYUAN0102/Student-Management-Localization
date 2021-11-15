using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Data;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly UserManager<User> userManager;

        public DashboardController(ApplicationDbContext dbcontext, UserManager<User> userManager)
        {
            this.dbcontext = dbcontext;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewBag.TotalUsers = this.dbcontext.Users.Count();
            ViewBag.TotalSubjects = this.dbcontext.Subjects.Count();
            ViewBag.TotalRooms = this.dbcontext.Rooms.Count();
            return View();
        }

        [HttpGet]
        public JsonResult GetProjects()
        {
            var Projects = this.dbcontext.Projects.AsEnumerable().GroupBy(x => x.Created.Month);
            return Json(new { data = Projects });
        }
    }
}
