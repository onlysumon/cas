namespace CAS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Submission = c.DateTime(nullable: false),
                        EvaluationStatus = c.String(maxLength: 20),
                        IsSubmitted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        ApplicantId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        MiddleName = c.String(maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        MarritalStatus = c.String(nullable: false, maxLength: 10),
                        DateOfBirth = c.DateTime(nullable: false),
                        CountryOfCitizen = c.String(nullable: false, maxLength: 10),
                        CountryOfBirth = c.String(nullable: false, maxLength: 2),
                        CurrentCity = c.String(nullable: false, maxLength: 2),
                        VisaStatus = c.String(nullable: false, maxLength: 5),
                        Skype = c.String(maxLength: 50),
                        PhoneHome = c.String(maxLength: 50),
                        PhoneWork = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ApplicantId);
            
            CreateTable(
                "dbo.AwardsAndHonors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        Applicant_ApplicantId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Applicant_ApplicantId)
                .Index(t => t.Applicant_ApplicantId);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstituteName = c.String(),
                        DegreeTitle = c.String(),
                        SubjectArea = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        GradingScale = c.Double(nullable: false),
                        FinalGpa = c.Double(nullable: false),
                        Applicant_ApplicantId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Applicant_ApplicantId)
                .Index(t => t.Applicant_ApplicantId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseType = c.String(),
                        Grade = c.String(),
                        Description = c.String(),
                        Education_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Educations", t => t.Education_Id)
                .Index(t => t.Education_Id);
            
            CreateTable(
                "dbo.Employments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComnapyName = c.String(nullable: false, maxLength: 50),
                        WebSite = c.String(maxLength: 50),
                        City = c.String(nullable: false, maxLength: 20),
                        Country = c.String(nullable: false, maxLength: 2),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        Position = c.String(nullable: false, maxLength: 30),
                        EmployementType = c.String(nullable: false, maxLength: 10),
                        Programming = c.Double(nullable: false),
                        DataStructur = c.Double(nullable: false),
                        Networking = c.Double(nullable: false),
                        DatabaseAdministration = c.Double(nullable: false),
                        Teaching = c.Double(nullable: false),
                        Management = c.Double(nullable: false),
                        Others = c.String(maxLength: 50),
                        OthersPercentage = c.Double(nullable: false),
                        Applicant_ApplicantId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Applicant_ApplicantId)
                .Index(t => t.Applicant_ApplicantId);
            
            CreateTable(
                "dbo.EnglishProficiencies",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Reading = c.Int(nullable: false),
                        Writting = c.Int(nullable: false),
                        Listening = c.Int(nullable: false),
                        Speaking = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.GREs",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        VerbalScore = c.Double(nullable: false),
                        VerbalPercentile = c.Double(nullable: false),
                        QuantitativeScore = c.Double(nullable: false),
                        QuantitativePercentile = c.Double(nullable: false),
                        AnalyticalScore = c.Double(nullable: false),
                        AnalyticalPercentile = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnglishProficiencies", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.IELTS",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        OverallScore = c.Double(nullable: false),
                        Reading = c.Double(nullable: false),
                        Writting = c.Double(nullable: false),
                        Listening = c.Double(nullable: false),
                        Speaking = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnglishProficiencies", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.TOEFLs",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnglishProficiencies", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        IsActive = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.User");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.User");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.User");
            DropForeignKey("dbo.Applications", "Id", "dbo.Applicants");
            DropForeignKey("dbo.TOEFLs", "Id", "dbo.EnglishProficiencies");
            DropForeignKey("dbo.IELTS", "Id", "dbo.EnglishProficiencies");
            DropForeignKey("dbo.GREs", "Id", "dbo.EnglishProficiencies");
            DropForeignKey("dbo.EnglishProficiencies", "Id", "dbo.Applicants");
            DropForeignKey("dbo.Employments", "Applicant_ApplicantId", "dbo.Applicants");
            DropForeignKey("dbo.Courses", "Education_Id", "dbo.Educations");
            DropForeignKey("dbo.Educations", "Applicant_ApplicantId", "dbo.Applicants");
            DropForeignKey("dbo.AwardsAndHonors", "Applicant_ApplicantId", "dbo.Applicants");
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Applications", new[] { "Id" });
            DropIndex("dbo.TOEFLs", new[] { "Id" });
            DropIndex("dbo.IELTS", new[] { "Id" });
            DropIndex("dbo.GREs", new[] { "Id" });
            DropIndex("dbo.EnglishProficiencies", new[] { "Id" });
            DropIndex("dbo.Employments", new[] { "Applicant_ApplicantId" });
            DropIndex("dbo.Courses", new[] { "Education_Id" });
            DropIndex("dbo.Educations", new[] { "Applicant_ApplicantId" });
            DropIndex("dbo.AwardsAndHonors", new[] { "Applicant_ApplicantId" });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.User");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TOEFLs");
            DropTable("dbo.IELTS");
            DropTable("dbo.GREs");
            DropTable("dbo.EnglishProficiencies");
            DropTable("dbo.Employments");
            DropTable("dbo.Courses");
            DropTable("dbo.Educations");
            DropTable("dbo.AwardsAndHonors");
            DropTable("dbo.Applicants");
            DropTable("dbo.Applications");
        }
    }
}
