using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Record> Records { get; set; }
        public DbSet<UserSubject> UserSubjects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserSubject>()
                .HasKey(x => new { x.UserId, x.SubjectId });
            modelBuilder.Entity<UserProject>()
                .HasKey(x => new { x.UserId, x.ProjectId });

            //foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(e => e.GetForeignKeys()))
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            //}
        }

    }

}