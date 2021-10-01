using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentManagement.Models;
using StudentManagement.Models.ManageViewModel;
using StudentManagement.Service;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;

        public ManageController(
          UserManager<User> userManager,
          SignInManager<User> signInManager,
          ILoggerFactory loggerFactory,
          IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = loggerFactory.CreateLogger<ManageController>();
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string? id)
        {
            if(id == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var user = await this.userManager.FindByIdAsync(id);

            if(user == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return this.View(user);
        }


        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            this.ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeEmailSuccess ? "Your email has been changed."
                 : message == ManageMessageId.ChangeUserNameSuccess ? "Your Username has been changed."
                  : message == ManageMessageId.Error ? "An error has occurred."
                   : message == ManageMessageId.AddPhoto ? "Your Profile Photo has been changed."
                    : "";

            var user = await this.GetCurrentUserAsync();

            if (user == null)
            {
                return this.View("Error");
            }

            ViewBag.ImagePath = user.ImagePath;

            return this.View(new ProfileViewModel
            {
                changeUserNameViewModel = new ChangeUserNameViewModel
                { 
                  FirstName = user.FirstName,
                  LastName = user.LastName
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            unitOfWork.UploadImage(file);
            if(this.ModelState.IsValid)
            {
                var user = await this.GetCurrentUserAsync();
                if(user != null)
                {
                    user.ImagePath = file.FileName;

                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ManageController.Index), new { Message = ManageMessageId.AddPhoto });
                    }
                }
            }
            return View();
        }

        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return this.View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.GetCurrentUserAsync();
            if (user != null)
            {
                var result = await this.userManager.ChangePasswordAsync(user, model.changePasswordViewModel.OldPassword, model.changePasswordViewModel.NewPassword);
                if (result.Succeeded)
                {
                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    this.logger.LogInformation(3, "User changed their password successfully.");
                    return this.RedirectToAction(nameof(ManageController.Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }

                this.AddErrors(result);
                return this.View(model);
            }

            return this.RedirectToAction(nameof(ManageController.Index), new { Message = ManageMessageId.Error });
        }

        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ProfileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.GetCurrentUserAsync();

            var token = await userManager.GenerateChangeEmailTokenAsync(user, model.changeEmailViewModel.NewEmail);

            logger.Log(LogLevel.Warning, token);
            if (user != null)
            {
                var result = await this.userManager.ChangeEmailAsync(user, model.changeEmailViewModel.NewEmail, token);

                if (result.Succeeded)
                {
                    user.UserName = model.changeEmailViewModel.NewEmail;
                    user.Email = model.changeEmailViewModel.NewEmail;
                    await this.userManager.UpdateAsync(user);
                    this.logger.LogInformation(4, "User changed their email successfully.");
                    return this.RedirectToAction(nameof(ManageController.Index), new { Message = ManageMessageId.ChangeEmailSuccess });
                }

                this.AddErrors(result);
                return this.View(model);
            }
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return this.RedirectToAction("Login","Account");
        }


        [HttpGet]
        public async Task<IActionResult> ChangeUserName()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            var model = new ProfileViewModel
            {
                changeUserNameViewModel = new ChangeUserNameViewModel
                { 
                  FirstName = user.FirstName,
                  LastName = user.LastName
                }
            };
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserName(ProfileViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if(ModelState.IsValid)
            {
                user.FirstName = model.changeUserNameViewModel.FirstName;
                user.LastName = model.changeUserNameViewModel.LastName;
                var result = await userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(ManageController.Index), new { Message = ManageMessageId.ChangeUserNameSuccess });
                }

            }
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            ChangeEmailSuccess,
            ChangePasswordSuccess,
            ChangeUserNameSuccess,
            Error,
            AddPhoto
        }

        private Task<User> GetCurrentUserAsync()
        {
            return this.userManager.GetUserAsync(this.HttpContext.User);
        }

        #endregion
    }

}
