using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class AwardsAndHonors
    {
        public int Id { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public Applicant Applicant { get; set; }
    }
}