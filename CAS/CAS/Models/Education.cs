using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class Education
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Institute name")]
        [MaxLength(30)]
        public string InstituteName { get; set; }

        [Required]
        [Display(Name = "Degree title")]
        [MaxLength(30)]
        public string DegreeTitle { get; set; }

        [Required]
        [Display(Name = "Degree status")]
        public char DegreeStatus { get; set; }
        public string SubjectArea { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double GradingScale { get; set; }
        public double FinalGpa { get; set; }

        public List<Course> Courses { get; set; }
        public Applicant Applicant { get; set; }
    }
}