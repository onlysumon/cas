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
        [MaxLength(10)]
        public char DegreeStatus { get; set; }

        [Required]
        [Display(Name = "Subject area")]
        [MaxLength(30)]
        public string SubjectArea { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(30)]
        public string City { get; set; }

        [Required]
        [MaxLength(30)]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "From date")]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "To date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Grading scale")]
        public double GradingScale { get; set; }

        [Display(Name = "Final GPA")]
        public double FinalGpa { get; set; }

        public string ApplicantId { get; set; }

        public List<Course> Courses { get; set; }
        public Applicant Applicant { get; set; }
    }
}