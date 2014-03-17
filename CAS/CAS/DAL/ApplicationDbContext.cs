using CAS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CAS.DAL
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext() : base("ComproAppSystem") { }

        //Othere DbSet will be added here to manage table in DB.
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Change the name of the table to be Users instead of AspNetUsers
            modelBuilder.Entity<IdentityUser>().ToTable("User");
            modelBuilder.Entity<AppUser>().ToTable("User");
        }

        public System.Data.Entity.DbSet<CAS.Models.Applicant> Applicants { get; set; }

        public System.Data.Entity.DbSet<CAS.Models.Application> Applications { get; set; }

        public System.Data.Entity.DbSet<CAS.Models.EnglishProficiency> EnglishProficiencies { get; set; }

        public System.Data.Entity.DbSet<CAS.Models.GRE> GREs { get; set; }

        public System.Data.Entity.DbSet<CAS.Models.IELTS> IELTS { get; set; }

        public System.Data.Entity.DbSet<CAS.Models.TOEFL> TOEFLs { get; set; }

        public System.Data.Entity.DbSet<CAS.Models.Education> Educations { get; set; }

        public System.Data.Entity.DbSet<CAS.Models.AwardsAndHonors> AwardsAndHonors { get; set; }

        public System.Data.Entity.DbSet<CAS.Models.Employment> Employments { get; set; }


    }
}