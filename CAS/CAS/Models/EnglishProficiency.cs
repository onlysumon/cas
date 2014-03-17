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
        
        [Display(Name = "How well can you read in English?")]
        public int Reading { get; set; }

        [Display(Name = "How well can you write in English?")]
        public int Writting { get; set; }

        [Display(Name = "How well do you understand spoken in English?")]
        public int Listening { get; set; }

        [Display(Name = "How well do you speak in English?")]
        public int Speaking { get; set; }

        public IELTS Ielts { get; set; }
        public GRE Gre { get; set; }
        public TOEFL Toefl { get; set; }
        public Applicant Applicant { get; set; }
    }
}