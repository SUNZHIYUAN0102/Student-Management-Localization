using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.ProjectViewModel;
using StudentManagement.Models.SubjectViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public NotificationsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? subjectId, SubjectDetailViewModel model)
        {
            if(subjectId == null)
            {
                return View("NotFound");
            }

            var subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == subjectId);

            if(subject == null)
            {
                return View("NotFound");
            }

            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(this.HttpContext.User);
                var now = DateTime.Now;
                var notification = new Notification()
                {
                    Text = model.NotificationCreateViewModel.Text,
                    Created = now,
                    SubjectId = subject.Id,
                    Creator = user
                };

                this.context.Notifications.Add(notification);
                await this.context.SaveChangesAsync();

                return RedirectToAction("Details", "Subjects", new { id = subject.Id });
            }

            return View();
        }
    }
}
