using System.Collections.Generic;
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

        public int CourseID { get; set; }//PK
        public string Title { get; set; }
        public int Credits { get; set; }

        //============ Navigation properties ===================== //
        //1 course: many enrollments
        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}