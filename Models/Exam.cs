using System;
using System.Collections.Generic;

namespace MoodleExam.Models
{
    public partial class Exam
    {
        public int Id { get; set; }
        public int Courseid { get; set; }
        public string Name { get; set; } = null!;
        public string Answers { get; set; } = null!;
        public int Timelimit { get; set; }
        public string ExamPdf { get; set; } = null!;
        public string AnswerPdf { get; set; } = null!;
        public DateTime Lastaccesstime { get; set; }
        public DateTime Firstaccesstime { get; set; }
    }
}
