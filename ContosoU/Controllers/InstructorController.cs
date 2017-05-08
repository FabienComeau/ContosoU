using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU.Data;
using ContosoU.Models;
using ContosoU.Models.SchoolViewModels;

namespace ContosoU.Controllers
{
    public class InstructorController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorController(SchoolContext context)
        {
            _context = context;    
        }

        // GET: Instructor
        public async Task<IActionResult> Index(int? id, int? courseID)//added parameter to select and instructor
                                                                      //added parameter to select course
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _context.Instructors
                .Include(i=>i.OfficeAssignment) //to include more data to the table assigned to Instuctor
                //==== enrollment ====//
                .Include(c=>c.Courses) //within courses property load the enrollments
                .ThenInclude(c=>c.Course) // have to get course entity out of the courses join entity
                .ThenInclude(d=>d.Department)
                .OrderBy(o=>o.LastName) //sort by instructor last name asc
                //.AsNoTracking() //improve performance
                .ToListAsync();

            //section for instructor selected
            if(id != null)//if passed an id for instructor
            {
                //========= get instructor
                Instructor instructor = viewModel.Instructors.Where(
                    i => i.ID == id.Value).Single();//return a Single instructor entity
                //now get instructor courses
                viewModel.Courses = instructor.Courses.Select(s => s.Course);

                //get instructor name for display in view
                ViewData["Instructor"] = instructor.FullName;

                //return the instructor id back to the view for highlighting selected row
                ViewData["InstructorID"] = id.Value; //this the passed in parameter
                //or
                //ViewData["InstructorID"] = instructor.ID; //current instructor id property
            }

            //========= course selected
            if(courseID != null)
            {
                //get all enrollment for this course (explicit loading: loading only if requested)
                _context.Enrollments.Include(i => i.Student).Where(c => c.CourseID == courseID.Value).Load();
                viewModel.Enrollments = viewModel.Courses.Where(x => x.CourseID == courseID).Single().Enrollments;
                
            }
            return View(viewModel);
            //original code
            //return View(await _context.Instructors.ToListAsync());

            ViewData["CourseID"] = courseID.Value;
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.Courses = new List<CourseAssignment>();
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HireDate,LastName,FirstName,Email,OfficeAssignment")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HireDate,ID,LastName,FirstName,Email")] Instructor instructor)
        {
            if (id != instructor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.SingleOrDefaultAsync(m => m.ID == id);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}
