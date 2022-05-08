using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Service
{
    public class UserPermission : IUserPermission
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public UserPermission(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        private HttpContext HttpContext => this.httpContextAccessor.HttpContext;

        public Boolean CanEditNote(Note note)
        {
            if ((this.HttpContext.User.IsInRole(UserRoles.Administrator))|| (this.HttpContext.User.IsInRole(UserRoles.Teacher)))
            {
                return true;
            }

            return this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == note.CreatorId;
        }
    }
}