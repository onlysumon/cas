using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class Employment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ComnapyName { get; set; }

        [MaxLength(50)]
        public string WebSite { get; set; }

        [Required]
        [MaxLength(20)]
        public string City { get; set; }

        [Required]
        [MaxLength(2)]
        public string Country { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string Position { get; set; }

        [Required]
        [MaxLength(10)]
        public string EmployementType { get; set; }

        [Required]
        public double Programming { get; set; }

        [Required]
        public double DataStructur { get; set; }

        [Required]
        public double Networking { get; set; }

        [Required]
        public double DatabaseAdministration { get; set; }
        
        [Required]
        public double Teaching { get; set; }

        [Required]
        public double Management { get; set; }

        [MaxLength(50)]
        public string Others { get; set; }

        public double OthersPercentage { get; set; }

        public Applicant Applicant { get; set; }
    }
}