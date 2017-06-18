using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSolutionModel
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(20)]
        [Display(Name = "Department Code")]
        [Required(ErrorMessage = "Your Department Code is required")]
        public string DeptCode { get; set; }

        [Display(Name = "Department Name")]
        [Required(ErrorMessage = "Your Department Name is required")]
        public string DeptName { get; set; }

        public virtual ICollection<Student> Students { get; set; }

    }
}
