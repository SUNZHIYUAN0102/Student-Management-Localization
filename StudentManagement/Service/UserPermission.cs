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

        public bool CanAddNote(Models.Project project)
        {
            var currentUser = this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User);
            return !project.Notes.Any(x => x.CreatorId == currentUser);
            
        }

        public bool CanChangePrivate(Models.Project project)
        {
            if ((this.HttpContext.User.IsInRole(UserRoles.Administrators)))
            {
                return true;
            }

            var currentUser = this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User);

            if(currentUser == project.CreatorId)
            {
                return true;
            }

            return false;
        }

        public Boolean CanEditNote(Note note)
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if ((this.HttpContext.User.IsInRole(UserRoles.Administrators)))
            {
                return true;
            }

            return this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == note.CreatorId;
        }

        public bool CanSeeProject(Models.Project project)
        {
            if(project.IsPrivate == false)
            {
                return true;
            }

            if ((this.HttpContext.User.IsInRole(UserRoles.Administrators)))
            {
                return true;
            }

            var currentUser = this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User);

            if ((project.IsPrivate == true) && (currentUser == project.CreatorId))
            {
                return true;
            }

            return false;
        }

        
    }
}