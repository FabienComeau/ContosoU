using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoU.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //you can turn off identity property by using the DatabaseGeneratorOption.None
        //you have the following 3 option:
        //Computed: Database generates a value when a row is inserted or updated.
        //Identity: Database generates a value when a row is inserted.
        //None: Database does not generate a value
        
        [Display(Name = "Number")]
        public int CourseID { get; set; }//PK

        [StringLength(50,MinimumLength =3)]
        [Required]
        public string Title { get; set; }

        [Range(0,5)]
        public int Credits { get; set; }

        [Display(Name ="Department")]
        public int DepartmentID { get; set; }//fk

        //============ Navigation properties ===================== //
        //1 course: many enrollments
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        // 1 course: many instructor
        public virtual ICollection<CourseAssignment> Assignments { get; set; }
        public virtual Department Department { get; set; }

        //calculated property
        //return the courseID and course title
        public string CourseIdTitle
        {
            get
            {
                return CourseID + ": " + Title;
            }
                
        }

    }
}