﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using ExamSolutionModel;

namespace CompunetCbte.ViewModels
{
    public class SetUpVm
    {
        [Display(Name = "School Name")]
        [Required(ErrorMessage = "School Name is required")]
        public string SchoolName { get; set; }

        [Display(Name = "Select School Color")]
        [Required(ErrorMessage = "Please Select A color Theme")]
        public ThemeColor SchoolTheme { get; set; }

        [Display(Name = "Upload A Passport/Picture")]
        [ExamSolutionModel.ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 1MB")]
        [NotMapped]
        public HttpPostedFileBase File { get; set; }

    }
}