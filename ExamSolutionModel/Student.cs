using ExamSolutionModel.CBTE;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamSolutionModel
{
    public class Student : Person
    {
        [Key]
        public string StudentId { get; set; }

        public int DepartmentId { get; set; }
        public string JambRegNo { get; set; }
        public string Password { get; set; }
        public bool IsLogin { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //[NotMapped]
        //public string ConfirmPassword { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<ExamLog> ExamLogs { get; set; }
        public virtual ICollection<StudentQuestion> StudentQuestions { get; set; }

    }
}