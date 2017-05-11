using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU.Data;
using ContosoU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ContosoU.Controllers
{
    [Authorize]
    [Authorize(Roles = "student")]//need to be logged in and part of student role
    public class StudentEnrollmentController : Controller
    {
        private readonly SchoolContext _context;
        //fcomeau: need identity user
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentEnrollmentController(SchoolContext context, UserManager<ApplicationUser> userManager)//fcomeau: instantiate the userManager
        {
            _context = context;
            _userManager = userManager;
        }     

        // GET: StudentEnrollment
        public async Task<IActionResult> Index()
        {
            //fcomeau: retrieve currently logged in student
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                //not log in
                return NotFound();
            }

            //locate logged in user within the student entity
            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()

                .SingleOrDefaultAsync(m => m.Email == user.Email);//associate identity user -> student using email property
            
            // 1. Courses Enrolled :  (student is enrolled in these)
            var studentEnrollments = _context.Enrollments
                                     .Include(c => c.Course)
                                     .Include(c => c.Student)
                                     .Where(c => c.StudentID == student.ID)
                                     .AsNoTracking();

            ViewData["StudentName"] = student.FullName;

            // 2. Courses Available: (student is NOT enrolled in these)
            string query = "SELECT * FROM Course WHERE CourseID NOT IN (SELECT DISTINTCT CourseID FROM Enrollment WHERE StudentID = {0}";
            //Building a RAW SQL Query using LINQ (Language intergrated query)
            var courses = _context.Courses
                .FromSql(query, student.ID)
                .AsNoTracking();

            //ViewData["Courses"] = courses.ToList();
            ViewBag.Courses = courses.ToList();

            return View(await studentEnrollments.ToListAsync());
;

        }
        //this was generated by the lightbulp from above(GetCurrentUserAsync();)
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        
        // GET: Enroll
        public async Task<IActionResult> Enroll(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            //get currently logged in student
            var user = await GetCurrentUserAsync();
            if(user == null)
            {
                return NotFound();
            }
            //locate the logged in user (student) within the student entity
            var student = await _context.Students
                .Include(s => s.Enrollments).ThenInclude(s => s.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Email == user.Email);
            //return student id to view using ViewData
            ViewData["StudentID"] = student.ID;//for hidden field in form

            //retrieve this student current enrollment for comparison with course they want to enroll
            var studentEnrollments = new HashSet<int>(_context.Enrollments
                .Include(e => e.Course)
                //.Include(e => e.Student)
                .Where(e => e.StudentID == student.ID)
                .Select(e => e.CourseID));//only select CourseID

            //convertsion from int? to int is not possible(need int)
            int currentCourseID;
            if(id.HasValue)//id is the method parameter
            {
                currentCourseID = (int)id;
            }
            else
            {
                currentCourseID = 0;
            }//end of conversion fix

            //situation where student tries to enroll in same course
            if(studentEnrollments.Contains(currentCourseID))
            {
                //Student is trying to enroll in a course already enrolled in(send a model stat error back to view)
                ModelState.AddModelError("AlreadyEnrolled", "You are already enrolled in this course!");
            }
            //situation where student tries to enroll in noexistant course
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseID == id);
            //if course was not instantiated because no course id was found based on param coming in
            if(course == null)
            {
                return NotFound();
            }
            //otherwise return the view(with course entity)

            return View(course);
        }

        // POST: Enroll
        public async Task<IActionResult> Enroll([Bind("CourseID, StudentID")] Enrollment enrollment)
        {
            _context.Add(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}