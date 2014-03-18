using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class IELTS
    {
        [Key, ForeignKey("EnglishProficiency")]
        public string Email { get; set; }

        [Display(Name = "Overall score")]
        public double? OverallScore { get; set; }

        public double? Reading { get; set; }
        public double? Writting { get; set; }
        public double? Listening { get; set; }
        public double? Speaking { get; set; }

        public virtual EnglishProficiency EnglishProficiency { get; set; }
    }
}