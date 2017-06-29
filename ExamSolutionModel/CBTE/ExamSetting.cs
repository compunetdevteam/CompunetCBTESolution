using System;
using System.ComponentModel.DataAnnotations;

namespace ExamSolutionModel.CBTE
{
    public class ExamSetting
    {
        public int ExamSettingId { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Course Name is required")]
        public int CourseId { get; set; }

        //public string SubjectName { get; set; }

        [Display(Name = "Semester")]
        [Required(ErrorMessage = "Semester is required")]
        public int SemesterId { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public int SessionId { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date)]
        public DateTime ExamDate { get; set; }

        [Display(Name = "Exam Start Time (Hours)")]
        [Range(0, 23)]
        public int ExamStartTime { get; set; }

        [Display(Name = "Exam End Time (Hours)")]
        [Range(0, 23)]
        public int ExamEndTime { get; set; }

        public int ExamTypeId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual Session Sessions { get; set; }
        public virtual ExamType ExamType { get; set; }
    }
}
