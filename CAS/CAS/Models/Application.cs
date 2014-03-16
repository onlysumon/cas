using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class Application
    {
        [Key, ForeignKey("Applicant")]
        public int Id { get; set; }        

        public DateTime StartDate { get; set; }
        public DateTime Submission { get; set; }

        [MaxLength(20)]
        public string EvaluationStatus { get; set; }

        public bool IsSubmitted { get; set; }
        
        public Applicant Applicant { get; set; }
    }
}