using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExamSolutionModel.CBTE;

namespace ExamSolutionModel
{
    public class Student : Person
    {
        [Key]
        public string StudentId { get; set; }

        public int DepartmentId { get; set; }
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<ExamLog> ExamLogs { get; set; }
    }
}