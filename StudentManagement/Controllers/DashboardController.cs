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
        private readonly RoleManager<IdentityRole> roleManager;

        public DashboardController(ApplicationDbContext dbcontext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbcontext = dbcontext;
            this.userManager = userManager;
            this.roleManager = roleManager;
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
            var Projects = this.dbcontext.Projects.OrderBy(x=>x.Created).AsEnumerable().GroupBy(x => x.Created.Month);
            return Json(new { data = Projects });
        }

        [HttpGet]
        public async Task<JsonResult> GetRoles()
        {
            List<MyRole> Roles = new List<MyRole>();
            var role = this.roleManager.Roles.ToList();
            foreach (var item in role)
            {
                var RolesUserlist = await this.userManager.GetUsersInRoleAsync(item.Name);
                Roles.Add(new MyRole() { RoleName = item.Name, Amount = RolesUserlist.Count });
            }

            return Json(new { data = Roles });
        }
    }
}
