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
        public int Id { get; set; }
        public double VerbalScore { get; set; }
        public double VerbalPercentile { get; set; }
        public double QuantitativeScore { get; set; }
        public double QuantitativePercentile { get; set; }
        public double AnalyticalScore { get; set; }
        public double AnalyticalPercentile { get; set; }

        public EnglishProficiency EnglishProficiency { get; set; }
    }
}