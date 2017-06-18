using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExamSolutionModel.CBTE;

namespace ExamSolutionModel
{
    public class Session
    {
        public int SessionId { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(20)]
        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session Name is required")]
        public string SessionName { get; set; }

        [Display(Name = "Session Start")]
        [Required(ErrorMessage = "Session Start is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Session End")]
        [Required(ErrorMessage = "Session End is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Current Session")]
        public bool ActiveSession { get; set; }

        public virtual ICollection<ExamRule> ExamRules { get; set; }
     
        public virtual ICollection<ExamSetting> ExamSettings { get; set; }
        public virtual ICollection<ExamLog> ExamLogs { get; set; }
       

    }

}