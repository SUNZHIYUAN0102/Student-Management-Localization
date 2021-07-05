using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.StudentViewModel;

namespace StudentManagement.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext context;

        public StudentsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var student = context.Students;
            return View(student);
        }

        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> CreateStudent(StudentEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                var student = new Student
                {
                    StudentName = model.StudentName,
                    GroupNumber = model.GroupNumber
                };

                this.context.Students.Add(student);
                await context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditStudent(Guid id)
        {
            var student = await this.context.Students.FindAsync(id);

            if(id == null)
            {
                return NotFound();
            }

            var model = new StudentEditViewModel
            {
                StudentName = student.StudentName,
                GroupNumber = student.GroupNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(Guid id, StudentEditViewModel model)
        {
            var student = await this.context.Students.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }

            if(this.ModelState.IsValid)
            {
                student.StudentName = model.StudentName;
                student.GroupNumber = model.GroupNumber;
            }
            await this.context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await this.context.Students.FindAsync(id);

            if(id == null)
            {
                return this.NotFound();
            }
            else
            {
                var result = context.Students.Remove(student);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
        }
    }
}
