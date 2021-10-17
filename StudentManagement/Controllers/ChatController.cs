using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.ChatViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public ChatController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var messages =await this.context.Messages
                .Include(x => x.Creator)
                .ToListAsync();
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            ViewBag.User = this.context.Users;
            ViewBag.Name = user.FullName;
            return View(new ChatIndexViewModel{ Messages=messages});
        }

    }
}
