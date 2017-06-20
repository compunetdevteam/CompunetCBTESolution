using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExamSolutionModel.CBTE;
using SwiftKampusModel;

namespace ExamSolutionModel
{
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(20)]
        public string SemesterName { get; set; }

        [Display(Name = "Current Semester")]
        public bool ActiveSemester { get; set; }
        //public virtual ICollection<Course> Courses { get; set; }
  
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<ExamSetting> ExamSettings { get; set; }
        public virtual ICollection<ExamLog> ExamLogs { get; set; }
       
    }
}
