using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CAS.Models
{
    public class Applicant
    {
        [Key]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First name")]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        [MaxLength(20)]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "I am ")]
        [MaxLength(2)]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Marrital status")]
        [MaxLength(10)]
        public string MarritalStatus { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of birth")]
        public System.DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Country of citizen")]
        [MaxLength(2)]
        public string CountryOfCitizen { get; set; }

        [Required]
        [Display(Name = "Country of birth")]
        [MaxLength(2)]
        public string CountryOfBirth { get; set; }

        [Required]
        [Display(Name = "Current city (City, Country)")]
        [MaxLength(30)]
        public string CurrentCity { get; set; }

        [Required]
        [Display(Name = "Visa status")]
        [MaxLength(5)]
        public string VisaStatus { get; set; }

        [MaxLength(50)]
        public string Skype { get; set; }

        [Display(Name = "Phone Home")]
        [MaxLength(50)]
        public string PhoneHome { get; set; }

        [Display(Name = "Phone Work")]
        [MaxLength(50)]
        public string PhoneWork { get; set; }

        [Display(Name = "Has applied before")]
        public bool IsAppliedBefore { get; set; }

        //Relations
        public Application Application { get; set; }
        public List<Education> Educations { get; set; }
        public List<Employment> Employments { get; set; }
        public List<AwardsAndHonors> AwardsAndHonors { get; set; }
        public EnglishProficiency EnglishProficiency { get; set; }        
    }
}