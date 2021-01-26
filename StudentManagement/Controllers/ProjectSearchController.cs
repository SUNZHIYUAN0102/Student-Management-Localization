using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Data;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectSearchController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ProjectSearchController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public IActionResult Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var projectTitle = dbContext.Projects.Where(p => p.Title.Contains(term))
                                            .Select(p => p.Title).ToList();
                return Ok(projectTitle);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
