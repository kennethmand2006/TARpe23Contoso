using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Department
    {
        [Key]

        public int DepartmentID { get; set; }
        [StringLength(50, MinimumLength = 3)]

        public string Name { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName  = "Money")]

        public decimal Budget { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime StartDate { get; set; }
        /*
         *  kaks oma andmetyypi osakonna jaoks
         */

        public Student? AStudent { get => aStudent; set => aStudent = value; } // minu isiklik annamida mitte yhelgi opilasel ei ole
        public int? StudentID { get; set; }
        [Display(Name = "Ancient scrolls which tell the tale of creation of this damned place.")]

        public string story { get; set; } //minu teine isiklik anne, Terve osakonna saaga - ehk kuidas byrokraatia in

        public int? InstructorID { get; set; }  
        [Timestamp]

        public byte? RowVersion {  get; set; } 
        private Student? aStudent {  get; set; }   

        public Instructor? Adminstrator { get; set; }   

        public ICollection<Course>? Courses { get; set; }



    }
}
