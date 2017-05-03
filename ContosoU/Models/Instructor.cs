using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models
{
    public class Instructor: Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        // ================ Navigation properties ============== //
        //an instructor can teach any number of courses, so courses is defined as a collection of the courseassignment entity
        public virtual ICollection<CourseAssignment> Courses { get; set; }
        //an instructor can only have at most one office,so the officeassignment property holds a single
        //officeassignment entity (which may be null if not office is assigned)
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}
