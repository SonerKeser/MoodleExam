using System;
using System.Collections.Generic;

namespace MoodleExam.Models
{
    public partial class Userexam
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public int Examid { get; set; }
        public string Answers { get; set; } = null!;
        public string Score { get; set; } = null!;
        public DateTime Starttime { get; set; }
        public DateTime Endtime { get; set; }
    }
}
