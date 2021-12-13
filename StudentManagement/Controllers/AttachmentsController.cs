using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class AttachmentsController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;

        public AttachmentsController(ApplicationDbContext dbcontext, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            this.dbcontext = dbcontext;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(Guid? projectId, IFormFile file)
        {
            if(projectId == null)
            {
                return NotFound();
            }

            var project = await this.dbcontext.Projects.SingleOrDefaultAsync(x => x.Id == projectId);

            if(project == null)
            {
                return NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            unitOfWork.UploadFile(file, user.Id);
            if (this.ModelState.IsValid)
            {
                var attachment = new Attachment
                {
                    FileName = user.Id + file.Name,
                    ProjectId = project.Id,
                    UserId = user.Id,
                    Created = DateTime.Now
                };

                this.dbcontext.Attachments.Add(attachment);

                await this.dbcontext.SaveChangesAsync();

                return RedirectToAction("Details", "Projects", new { id = project.Id });
            }

            return View();
        }
    }
}
