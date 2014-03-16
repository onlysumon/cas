using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class EnglishProficiency
    {
        [Key, ForeignKey("Applicant")]
        public string Email { get; set; }
        public int Reading { get; set; }
        public int Writting { get; set; }
        public int Listening { get; set; }
        public int Speaking { get; set; }

        public IELTS Ielts { get; set; }
        public GRE Gre { get; set; }
        public TOEFL Toefl { get; set; }
        public Applicant Applicant { get; set; }
    }
}