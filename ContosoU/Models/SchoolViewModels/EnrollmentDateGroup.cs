using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models.SchoolViewModels
{
    public class EnrollmentDateGroup
    {
        /*this class is considered to be a ViewModel. A ViewModel allows you to shape multiple entities into a single object, 
         * optimized for consumption and rendering by the view.  the purpose of the view Model is for the view to have
         * a single object to render.
         */ 

        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

       
        public int StudentCount { get; set; }
    }
}
