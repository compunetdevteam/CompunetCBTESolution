using ExamSolutionModel.CBTE;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamSolutionModel
{
    public class Course
    {
        public int CourseId { get; set; }

        [Display(Name = "Course Code")]
        [Required(ErrorMessage = "Your Course Code is required")]
        [StringLength(10, MinimumLength = 3)]
        public string CourseCode { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required(ErrorMessage = "Your Course Name is required")]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Course Description")]
        public string CourseDescription { get; set; }

        //public string CourseType { get; set; }

        [Range(1, 5)]
        [Required(ErrorMessage = "Your Course Credit is required")]
        public int Credits { get; set; }

        public virtual ICollection<DepartmentCourse> DepartmentCourses { get; set; }

        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<ExamRule> ExamRules { get; set; }

        public virtual ICollection<ExamSetting> ExamSettings { get; set; }
        public virtual ICollection<ExamLog> ExamLogs { get; set; }
        public virtual ICollection<ExamInstruction> ExamInstructions { get; set; }

    }


}