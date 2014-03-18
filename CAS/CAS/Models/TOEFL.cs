using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class TOEFL
    {
        [Key, ForeignKey("EnglishProficiency")]
        public string Email { get; set; }
        public int? Year { get; set; }
        public double? Score { get; set; }

        public virtual EnglishProficiency EnglishProficiency { get; set; }
    }
}