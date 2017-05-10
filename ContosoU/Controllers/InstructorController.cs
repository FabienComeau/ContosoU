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
                .Include(i => i.OfficeAssignment) //to include more data to the table assigned to Instuctor
                                                  //==== enrollment ====//
                .Include(c => c.Courses) //within courses property load the enrollments
                .ThenInclude(c => c.Course) // have to get course entity out of the courses join entity
                .ThenInclude(d => d.Department)
                .OrderBy(o => o.LastName) //sort by instructor last name asc
                                          //.AsNoTracking() //improve performance
                .ToListAsync();

            //section for instructor selected
            if (id != null)//if passed an id for instructor
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
            if (courseID != null)
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
            // populate the AssignedCourseData View Model
            PopulateAssignedCourseData(instructor);
            return View();
        }
        //this was created by using the light bulp up above(this is for your check boxes)
        private void PopulateAssignedCourseData(Instructor instructor)
        {
            //get all courses
            var allCourses = _context.Courses;

            //create a hashset of instructor courses (Hashset of intergers populated with course id)
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));

            //create and populate the AssignedCourseData ViewModel
            var viewModel = new List<AssignedCourseData>(); //created

            //populate once for each of the curses within allcourses
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            // save the viewModel within the viewDAta object for use within the view
            ViewData["Courses"] = viewModel;
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HireDate,LastName,FirstName,Email,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            //fcomeau: added string[] selectedCourses method argument for many course assignments (up above)
            if(selectedCourses != null)
            {
                //selectedCourses checkboxes have been checked - create a new list of CourseAssignment
                instructor.Courses = new List<CourseAssignment>();
                //loop the selectedCouses array
                foreach(var course in selectedCourses)
                {
                    //populate the CourseAssignment (InstructorID, CourseID)
                    var courseToAdd = new CourseAssignment
                    {
                        InstructorID = instructor.ID,
                        CourseID = int.Parse(course)
                    };
                    instructor.Courses.Add(courseToAdd); //add the new course to collection
                }
            }
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

            var instructor = await _context.Instructors
                .Include(i=>i.OfficeAssignment) //include officeAssignment
                .Include(i=>i.Courses) //insclude courses for assignmentCourseData viewModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            // populate the AssignedCourseData View Model
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        {
            //fcomeau: take care of overposting - added selectedCourse string array argument
            if (id == null)
            {
                return NotFound();
            }

            //find instructor to update(for overposting check)
            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .ThenInclude(i => i.Course) //for update of course
                .SingleOrDefaultAsync(i => i.ID == id); //only one instructor to update
            if(await TryUpdateModelAsync<Instructor>(
                instructorToUpdate, "", i=>i.FirstName, i=>i.LastName, i=>i.HireDate, i=>i.OfficeAssignment))
            {
                //to do: check for empty string on office location
                if(string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                {
                    instructorToUpdate.OfficeAssignment = null; //remove the complete record
                }
                //to do: update courses
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                //to do: save changes (try...catch)
                if(ModelState.IsValid)
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch(DbUpdateException)
                    {
                        //we could log the error using the ex argument - lets simply return a model state error back to the view
                        ModelState.AddModelError("", "unable to save changes.");
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(instructorToUpdate);
          
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if(selectedCourses == null)
            {
                //if no checkboxes were selected, initialize the courses navigation property with an empty collection and return
                instructorToUpdate.Courses = new List<CourseAssignment>();
                return;
            }
            //to facilitate efficent lookups, 2 collection will be sorted in Hashset objects
            //: selectedCoursesHS -> selected course (hashset of checkvoxe selections)
            //: instructorCourses -> instructor courses (hashset of courses assigned to instructor)
            var selectedCourseHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.Course.CourseID));

            //loop through all courses in the database and check each course against the one currently assigned
            // to the instructor versus the one taht were selected in the view
            foreach (var course in _context.Courses)//Loop all courses
            {
                //CONDITION 1:
                //If the checkbox for a course was selected but the course isn't in the 
                //Instructor.Courses navigation property, the course is added to the collection
                //in the navigation property
                if (selectedCourseHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Add(new CourseAssignment
                        {
                            InstructorID = instructorToUpdate.ID,
                            CourseID = course.CourseID
                        });
                    }
                }
                //CONDITION 2:
                //If the check box for a course wasn't selected, but the course is in the 
                //Instructor.Courses navigation property, the course is removed 
                //from the navigation property.
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        CourseAssignment courseToRemove =
                            instructorToUpdate.Courses
                            .SingleOrDefault(i => i.CourseID == course.CourseID);
                        _context.Remove(courseToRemove);
                    }
                }

            }//end foreach
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i=>i.OfficeAssignment)
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
