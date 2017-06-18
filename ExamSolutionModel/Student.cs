using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExamSolutionModel.CBTE;

namespace ExamSolutionModel
{
    public class Student : Person
    {
        [Key]
        public string StudentId { get; set; }

        public int DepartmentId { get; set; }
        public string Password { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<ExamLog> ExamLogs { get; set; }
    }
}