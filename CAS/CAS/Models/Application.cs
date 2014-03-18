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
        public String Email { get; set; }        

        public DateTime StartDate { get; set; }
        public DateTime SubmissionDate { get; set; }

        [MaxLength(20)]
        public string EvaluationStatus { get; set; }

        public bool IsSubmitted { get; set; }

        public virtual Applicant Applicant { get; set; }
    }
}