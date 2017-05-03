namespace ContosoU.Models
{

    //Grade enumeration
    public enum Grade
    {
        A,B,C,D,F
    }
    public class Enrollment
    {
        public int EnrollmentID { get; set; }//PK
        /*
         * the CourseID property is a foreign key, and the corresponding navigation property is Course. an Enrollment Entity is assosiated with
         * one Course Entity
         */ 
        public int CourseID { get; set; }//FK it knows automaticaly its a foreign key
        public int StudentID { get; set; }//FK
        /*
         * the studentID property is a foreign key, and the corresponding navigation property is Student, an Enrollment Entity is assosiated with
         * Student entity, so the property can only hold a single Student entity.
         * 
         * Entity Framework interprets a property as a foreign key property if it's named StudentID for the Student navigaiton property,
         * since the Student entity's primary key is ID (inherits form Person Entity ID Property in this case)
         * 
         * foreigh key properies can also be named simple <primary key property name> for example,
         * CourseID, since the Course Entity primary key is CourseID.
         */

        public Grade? Grade { get; set; }//? mean nullable: because we don't get a grade when registering

        // ==================== Navigation properties ============================//
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
 
 