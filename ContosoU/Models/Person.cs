using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Models
{
    public abstract class Person
    {
        //fcomeau: create the data models
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        //create a read only (calculated property with a get accessor only - will no get generated in database
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

    }
}
