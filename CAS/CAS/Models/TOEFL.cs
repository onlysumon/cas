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
        public int Id { get; set; }
        public int Year { get; set; }
        public double Score { get; set; }

        public EnglishProficiency EnglishProficiency { get; set; }
    }
}