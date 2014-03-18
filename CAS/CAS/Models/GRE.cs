using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class GRE
    {
        [Key, ForeignKey("EnglishProficiency")]
        public string Email { get; set; }
        public int? Year { get; set; }

        [Display(Name = "Verbal score")]
        public double? VerbalScore { get; set; }

        [Display(Name = "Verbal percentile")]
        public double? VerbalPercentile { get; set; }

        [Display(Name = "Quantitative score")]
        public double? QuantitativeScore { get; set; }

        [Display(Name = "Quantitative percentile")]
        public double? QuantitativePercentile { get; set; }

        [Display(Name = "Analytical score")]
        public double? AnalyticalScore { get; set; }

        [Display(Name = "Analytical percentile")]
        public double? AnalyticalPercentile { get; set; }

        public virtual EnglishProficiency EnglishProficiency { get; set; }
    }
}