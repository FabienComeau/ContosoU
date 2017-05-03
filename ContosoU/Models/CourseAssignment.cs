using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models
{
    public class CourseAssignment
    {
        public int InstructorID { get; set; }//composite PK, and also FK
        public int CourseID { get; set; }//composite PK, and also FK
        /*we could label both properties with the [key] attribute to create a composite PK
         * but we will do it using Fluent API within the schoolContext class
         */ 

        //======== navigation property =========//
        /*Many to Many between instructor and courses
         * Many intructor teaching many courses
         * 1 Course Many Course Assignments
         * 1 Instructor Many Course Assignments
         */ 
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
    }
}
