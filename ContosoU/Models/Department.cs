using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }//pk

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]//for client only
        [Column(TypeName = "money")]//sql server money data type
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        //relationship to instructor
        //a department may have an administrator(Instructor), and an administrator is always an instructor
        public int? InstructorID { get; set; }//? may be nullable

        //======= navigation properties ===========//
        //administrator is always an instructor
        public virtual Instructor Administrator { get; set; }
        //one department with many courses
        public virtual ICollection<Course> Courses { get; set; }
        //to do: handle Concurrency conflict (Add optimistic Concurrency)
    }
}
