using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models
{
    public class Instructor: Person
    {
        public DateTime HireDate { get; set; }

        // ================ Navigation properties ============== //
        //public virtual IColletciton<CourseAssignments> Courses { get; set; }
        //public virtual OfficeAssignemt OfficeAssignment { get; set; }
    }
}
