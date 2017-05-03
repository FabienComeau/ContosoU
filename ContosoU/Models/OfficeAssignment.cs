using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models
{
    public class OfficeAssignment
    {
        [Key]
        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }
        /*there is a one to zero or one relationship between the insructor and the officeassignment entities
         * an officeAsssignment only exists in relation to the instructor it's assigned to, and therefore it's
         * primary key is also it's foreign key to the instructor entity
         * 
         * problem: entity framework cannot automatically recognize intructorID as the primary key of this entity because its name 
         * doesn't follow the ID or ClassnameID naming convention.
         * 
         * therefore, the key attribute is used to identify it as the key.
         */ 
         [StringLength(50)]
         [Display(Name ="Office Location")]
         public string Location { get; set; }

        //========= Navigation property ==========//
        public virtual Instructor Instructor { get; set; }

    }
}
