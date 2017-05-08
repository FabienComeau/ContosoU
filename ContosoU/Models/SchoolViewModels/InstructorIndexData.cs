using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models.SchoolViewModels
{
    public class InstructorIndexData
    {
        /* the instructor index page will show data from three differnet tables.
         * so for this reason we are create the InstructorIndexData ViewModel. 
         * it will include three properties each holding the data for one of the following table:
         * - instructor
         * - course
         * - enrollment
         */

        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
