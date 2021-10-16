using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using StudentManagement.Data;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Service
{
    public class ChatHub : Hub
    {
        public readonly ApplicationDbContext dbcontext;
        public readonly UserManager<User> userManager;

        public ChatHub(ApplicationDbContext dbcontext, UserManager<User> userManager)
        {
            this.dbcontext = dbcontext;
            this.userManager = userManager;
        }
        public async Task SendMessage(string message, string creator)
        {
            var now = DateTime.Now;
            var user = await this.userManager.GetUserAsync(this.Context.User);
            var sendText = new Message
            {
                Creator = user,
                Text = message,
                Created = now
            };
            this.dbcontext.Messages.Add(sendText);
            await this.dbcontext.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", message, creator);
        }
    }
}
