namespace Cuentos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Imagebles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Filename = c.String(),
                        ContentType = c.String(),
                        Size = c.Int(nullable: false),
                        ImagebleId = c.Int(nullable: false),
                        Target = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Imagebles", t => t.ImagebleId, cascadeDelete: true)
                .Index(t => t.ImagebleId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Age = c.Int(),
                        GradeId = c.Int(),
                        Owner = c.String(nullable: false),
                        SchoolId = c.Int(),
                        PasswordHash = c.Binary(maxLength: 64),
                        PasswordSalt = c.Binary(maxLength: 128),
                        Email = c.String(nullable: false, maxLength: 200),
                        Comment = c.String(maxLength: 200),
                        IsApproved = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastLogin = c.DateTime(),
                        DateLastActivity = c.DateTime(),
                        DateLastPasswordChange = c.DateTime(nullable: false),
                        ApprovedDate = c.DateTime(),
                        ApprovedBy = c.String(),
                        Featured = c.Boolean(nullable: false),
                        ImageHolders_Id = c.Int(),
                    })
                .PrimaryKey(t => t.UserName)
                .ForeignKey("dbo.Grades", t => t.GradeId)
                .ForeignKey("dbo.Imagebles", t => t.ImageHolders_Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .Index(t => t.GradeId)
                .Index(t => t.SchoolId)
                .Index(t => t.ImageHolders_Id);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Name_EN = c.String(),
                        Position = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoryId = c.Int(nullable: false),
                        UserName = c.String(maxLength: 100),
                        Text = c.String(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedDate = c.DateTime(),
                        ApprovedBy = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stories", t => t.StoryId)
                .ForeignKey("dbo.Users", t => t.UserName)
                .Index(t => t.StoryId)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoryId = c.Int(nullable: false),
                        UserName = c.String(maxLength: 100),
                        Rate = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stories", t => t.StoryId)
                .ForeignKey("dbo.Users", t => t.UserName)
                .Index(t => t.StoryId)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.RoleName);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        SchoolId = c.Int(),
                        Subject = c.String(),
                        Comments = c.String(),
                        isRead = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.ImageCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoryCategories",
                c => new
                    {
                        StoryId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoryId, t.CategoryId })
                .ForeignKey("dbo.Stories", t => t.StoryId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.StoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.StoryGrades",
                c => new
                    {
                        StoryId = c.Int(nullable: false),
                        GradeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoryId, t.GradeId })
                .ForeignKey("dbo.Stories", t => t.StoryId, cascadeDelete: true)
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: true)
                .Index(t => t.StoryId)
                .Index(t => t.GradeId);
            
            CreateTable(
                "dbo.StoryInterests",
                c => new
                    {
                        StoryId = c.Int(nullable: false),
                        InterestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoryId, t.InterestId })
                .ForeignKey("dbo.Stories", t => t.StoryId, cascadeDelete: true)
                .ForeignKey("dbo.Interests", t => t.InterestId, cascadeDelete: true)
                .Index(t => t.StoryId)
                .Index(t => t.InterestId);
            
            CreateTable(
                "dbo.UserInterests",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 100),
                        InterestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserName, t.InterestId })
                .ForeignKey("dbo.Users", t => t.UserName, cascadeDelete: true)
                .ForeignKey("dbo.Interests", t => t.InterestId, cascadeDelete: true)
                .Index(t => t.UserName)
                .Index(t => t.InterestId);
            
            CreateTable(
                "dbo.RoleMemberships",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 100),
                        RoleName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => new { t.UserName, t.RoleName })
                .ForeignKey("dbo.Users", t => t.UserName, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleName, cascadeDelete: true)
                .Index(t => t.UserName)
                .Index(t => t.RoleName);
            
            CreateTable(
                "dbo.BuilderGalleries",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        UserName = c.String(maxLength: 100),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Imagebles", t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserName)
                .Index(t => t.Id)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.PageTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Active = c.Boolean(nullable: false),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Imagebles", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Details = c.String(nullable: false),
                        Address1 = c.String(nullable: false),
                        Address2 = c.String(),
                        CityId = c.Int(nullable: false),
                        Zip = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Imagebles", t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Stories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserName = c.String(maxLength: 100),
                        Name = c.String(nullable: false),
                        Featured = c.Boolean(nullable: false),
                        Summary = c.String(),
                        Pages = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        ApprovedDate = c.DateTime(),
                        ApprovedBy = c.String(),
                        Status = c.Int(nullable: false),
                        Views = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Imagebles", t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserName)
                .Index(t => t.Id)
                .Index(t => t.UserName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stories", "UserName", "dbo.Users");
            DropForeignKey("dbo.Stories", "Id", "dbo.Imagebles");
            DropForeignKey("dbo.Schools", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Schools", "Id", "dbo.Imagebles");
            DropForeignKey("dbo.PageTypes", "Id", "dbo.Imagebles");
            DropForeignKey("dbo.BuilderGalleries", "UserName", "dbo.Users");
            DropForeignKey("dbo.BuilderGalleries", "Id", "dbo.Imagebles");
            DropForeignKey("dbo.Contacts", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.Users", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.RoleMemberships", "RoleName", "dbo.Roles");
            DropForeignKey("dbo.RoleMemberships", "UserName", "dbo.Users");
            DropForeignKey("dbo.UserInterests", "InterestId", "dbo.Interests");
            DropForeignKey("dbo.UserInterests", "UserName", "dbo.Users");
            DropForeignKey("dbo.Users", "ImageHolders_Id", "dbo.Imagebles");
            DropForeignKey("dbo.Users", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.Ratings", "UserName", "dbo.Users");
            DropForeignKey("dbo.Ratings", "StoryId", "dbo.Stories");
            DropForeignKey("dbo.StoryInterests", "InterestId", "dbo.Interests");
            DropForeignKey("dbo.StoryInterests", "StoryId", "dbo.Stories");
            DropForeignKey("dbo.StoryGrades", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.StoryGrades", "StoryId", "dbo.Stories");
            DropForeignKey("dbo.Comments", "UserName", "dbo.Users");
            DropForeignKey("dbo.Comments", "StoryId", "dbo.Stories");
            DropForeignKey("dbo.StoryCategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.StoryCategories", "StoryId", "dbo.Stories");
            DropForeignKey("dbo.Images", "ImagebleId", "dbo.Imagebles");
            DropIndex("dbo.Stories", new[] { "UserName" });
            DropIndex("dbo.Stories", new[] { "Id" });
            DropIndex("dbo.Schools", new[] { "CityId" });
            DropIndex("dbo.Schools", new[] { "Id" });
            DropIndex("dbo.PageTypes", new[] { "Id" });
            DropIndex("dbo.BuilderGalleries", new[] { "UserName" });
            DropIndex("dbo.BuilderGalleries", new[] { "Id" });
            DropIndex("dbo.RoleMemberships", new[] { "RoleName" });
            DropIndex("dbo.RoleMemberships", new[] { "UserName" });
            DropIndex("dbo.UserInterests", new[] { "InterestId" });
            DropIndex("dbo.UserInterests", new[] { "UserName" });
            DropIndex("dbo.StoryInterests", new[] { "InterestId" });
            DropIndex("dbo.StoryInterests", new[] { "StoryId" });
            DropIndex("dbo.StoryGrades", new[] { "GradeId" });
            DropIndex("dbo.StoryGrades", new[] { "StoryId" });
            DropIndex("dbo.StoryCategories", new[] { "CategoryId" });
            DropIndex("dbo.StoryCategories", new[] { "StoryId" });
            DropIndex("dbo.Contacts", new[] { "SchoolId" });
            DropIndex("dbo.Ratings", new[] { "UserName" });
            DropIndex("dbo.Ratings", new[] { "StoryId" });
            DropIndex("dbo.Comments", new[] { "UserName" });
            DropIndex("dbo.Comments", new[] { "StoryId" });
            DropIndex("dbo.Users", new[] { "ImageHolders_Id" });
            DropIndex("dbo.Users", new[] { "SchoolId" });
            DropIndex("dbo.Users", new[] { "GradeId" });
            DropIndex("dbo.Images", new[] { "ImagebleId" });
            DropTable("dbo.Stories");
            DropTable("dbo.Schools");
            DropTable("dbo.PageTypes");
            DropTable("dbo.BuilderGalleries");
            DropTable("dbo.RoleMemberships");
            DropTable("dbo.UserInterests");
            DropTable("dbo.StoryInterests");
            DropTable("dbo.StoryGrades");
            DropTable("dbo.StoryCategories");
            DropTable("dbo.ImageCategories");
            DropTable("dbo.Contacts");
            DropTable("dbo.Roles");
            DropTable("dbo.Ratings");
            DropTable("dbo.Interests");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
            DropTable("dbo.Grades");
            DropTable("dbo.Users");
            DropTable("dbo.Cities");
            DropTable("dbo.Images");
            DropTable("dbo.Imagebles");
        }
    }
}
