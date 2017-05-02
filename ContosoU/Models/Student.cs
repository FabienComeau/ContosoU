using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models
{
    public class Student: Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        //================================ navigation property ============================================//
        /* -the enrollments property is a navigation property. Navigation properties hold other entities that are related to this entity.
         * -in this case, the enrollments property of a student entity will hold all of the enrollments that are related to that student.
         * -in onter words, if a given student row in the database has two related enrollment rows(rows tha contain that student primary key value
         *  in their studentId foreign key column), that student entity enrollment navigation property will contain thise two enrollment entities.
         *  
         *  Navigation properties are typically defined as virtual so that they can take advantage of certain entity Framework functionality
         *  such as lazy loading.
         *  note: Lazy Loading is not yet available in EF core.
         */

        public virtual ICollection<Enrollment> Enrollments { get; set; }//1 student: many enrollment

    }
}
