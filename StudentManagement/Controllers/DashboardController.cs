﻿using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Administrator")]
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
            List<RoleInformation> roleInformation = new List<RoleInformation>();
            var AllRoles = this.roleManager.Roles.ToList();
            foreach (var item in AllRoles)
            {
                var RolesUserlist = await this.userManager.GetUsersInRoleAsync(item.Name);
                roleInformation.Add(new RoleInformation() { RoleName = item.Name, Amount = RolesUserlist.Count });
            }

            return Json(new { data = roleInformation });
        }
    }
}
