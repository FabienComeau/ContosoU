using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models.SchoolViewModels
{
    public class AssignedCourseData
    {
        /* to provide a list of course check box with course id and title as well as an indiator that the instructor is assigned
         * or not assigned to a particular course, we are creating this viewmodel class
         */ 
        public int CourseID { get; set; } //for course id
        public string Title { get; set; } //for course title
        public bool Assigned { get; set; } //for is instructor assigned or not this course?
    }
}
