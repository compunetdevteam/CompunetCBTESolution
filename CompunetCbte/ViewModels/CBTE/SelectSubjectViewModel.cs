using System.ComponentModel.DataAnnotations;

namespace CompunetCbte.ViewModels.CBTE
{
    public class SelectSubjectViewModel
    {
        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        public int SubjectName { get; set; }

        public int ExamTypeId { get; set; }
    }
}