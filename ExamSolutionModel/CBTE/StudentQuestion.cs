﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSolutionModel.CBTE
{
    public class StudentQuestion
    {
        public int StudentQuestionId { get; set; }
        public string StudentId { get; set; }
      
        public int CourseId { get; set; }
        // public int LevelId { get; set; }
        public int SemesterId { get; set; }
        public int SessionId { get; set; }
        public int ExamTypeId { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }

        public bool Check1 { get; set; }
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }
        public bool Check4 { get; set; }
        public string FilledAnswer { get; set; }
        public string Answer { get; set; }
        public string QuestionHint { get; set; }

        public int QuestionNumber { get; set; }

        public bool IsCorrect { get; set; }
        public bool IsFillInTheGag { get; set; }
        public bool IsMultiChoiceAnswer { get; set; }
        public string SelectedAnswer { get; set; }
        public int TotalQuestion { get; set; }

        public int ExamTime { get; set; }

        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
        public virtual ExamType ExamType { get; set; }

    }
}