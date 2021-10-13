using Microsoft.AspNetCore.Authorization;
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

            var project = await this.context.Projects.SingleOrDefaultAsync(x => x.Id == projectId);

            if(project == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var getWeek = Week.Split(project.StartTime, project.DeadLine);

 
            //ViewBag.Students = new SelectList(students, "FullName", "Id");

            ViewBag.Week = new SelectList(getWeek);

            ViewBag.projectId = project.Id;

            return View(new RecordCreateViewModel());
        }
    }
}
