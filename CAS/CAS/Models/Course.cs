using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course type")]
        [MaxLength(10)]
        public string CourseType { get; set; }

        [Required]
        [MaxLength(2)]
        public string Grade { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public virtual Education Education { get; set; }
    }
}