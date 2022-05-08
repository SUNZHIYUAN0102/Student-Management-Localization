using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpGet]
        public async Task<IActionResult> Index(Guid? projectId)
        {
            if (projectId == null)
            {
                return NotFound();
            }

            var project = await this.dbcontext.Projects.SingleOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
            {
                return NotFound();
            }

            ViewBag.projectName = project.Title;

            var attachments = await this.dbcontext.Attachments.Include(x => x.User).Where(x => x.Project == project).ToListAsync();
            return View(attachments);
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
                    FileName = user.Id + file.FileName,
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

        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AssignScore(Guid? attachmentId, int score)
        {
            if (attachmentId == null)
            {
                return NotFound();
            }

            var attachment = await this.dbcontext.Attachments.SingleOrDefaultAsync(x => x.Id == attachmentId);

            if (attachment == null)
            {
                return NotFound();
            }

            if (this.ModelState.IsValid)
            {
                attachment.Score = score;
                await this.dbcontext.SaveChangesAsync();

                return RedirectToAction("Index","Attachments", new { projectId = attachment.ProjectId});
            }

            return this.View();
        }
    }
}
