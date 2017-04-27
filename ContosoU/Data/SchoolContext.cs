using ContosoU.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Data
{
    //Create the SchoolContext (database context for our University Database)
    public class SchoolContext: DbContext//this allow to use the entityframework and talk to database
    {
        //this is the constructor
        public SchoolContext(DbContextOptions<SchoolContext>option):base(option)
        {

        }

        //create your object(wich are a set of)(entity sets - corresponding to database tables and each single entity corresponding to a row
        public DbSet<Person> People { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        /*
         * when the database is created, EF(entity frameworks) creates table that have names the same as the DbSet property names.
         * Property names for collections are typically plural(Students rather than Student)
         * 
         * Developers disagree about whether table names should be pluralized or not.
         * For this demo, let's override the default dehavior         
         */
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
        }
    }
}
