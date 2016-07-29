using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1412695474)]
    public class CreateInitialDatabase : Migration
    {
        public override void Up()
        {
            Execute.Script("Migrate/Scripts/CreateUserMembershipTables.sql");
            this.CreateImagesTable();
            this.CreateCitiesTable();
            this.CreateSchoolsTable();
            this.CreateGradesTable();
            this.CreateInterestsTable();
            this.CreateUserInterestsTable();
            this.AlterUserTable();
            this.CreateStoriesTable();
            this.CreateCommentsTable();
            this.CreateRatingsTable();
        }

        public override void Down()
        {
            Delete.Table("RoleMemberships");
            Delete.Table("UserInterests");
            Delete.Table("Comments");
            Delete.Table("Ratings");
            Delete.Table("Stories");
            Delete.Table("Users");
            Delete.Table("Roles");
            Delete.Table("Schools");
            Delete.Table("Cities");
            Delete.Table("Grades");
            Delete.Table("Interests");
            Delete.Table("Images");
            Delete.Table("Imagebles");
        }

        private void CreateImagesTable()
        {
            Create.Table("Imagebles")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity();

            Create.Table("Images")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ImagebleId").AsInt32().NotNullable().ForeignKey("Imagebles", "Id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("Filename").AsString().Nullable()
                .WithColumn("ContentType").AsString().Nullable()
                .WithColumn("Size").AsInt32().Nullable()
                .WithColumn("Position").AsInt16().Nullable()
                .WithColumn("Target").AsString().Nullable()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        private void AlterUserTable()
        {
            Alter.Table("Users")
                .AddColumn("Name").AsString().Nullable()
                .AddColumn("LastName").AsString().Nullable()
                .AddColumn("Age").AsInt32().Nullable()
                .AddColumn("GradeId").AsInt32().ForeignKey("Grades", "Id").Indexed().Nullable()
                .AddColumn("SchoolId").AsInt32().ForeignKey("Schools", "Id").Indexed().Nullable()
                .AddColumn("ImageHolders_Id").AsInt32().ForeignKey("Imagebles", "Id").Nullable();
        }

        private void CreateCitiesTable()
        {
            Create.Table("Cities")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        private void CreateSchoolsTable()
        {
            Create.Table("Schools")
                .WithColumn("Id").AsInt32().PrimaryKey().ForeignKey("Imagebles", "Id").OnDelete(System.Data.Rule.Cascade).Indexed()
                .WithColumn("Name").AsString().Indexed().NotNullable()
                .WithColumn("Details").AsString().Nullable()
                .WithColumn("Address1").AsString().Nullable()
                .WithColumn("Address2").AsString().Nullable()
                .WithColumn("CityId").AsInt32().ForeignKey("Cities", "Id").Indexed().NotNullable()
                .WithColumn("Zip").AsString()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        private void CreateGradesTable()
        {
            Create.Table("Grades")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Name_EN").AsString().Nullable()
                .WithColumn("Position").AsInt32().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        private void CreateInterestsTable()
        {
            Create.Table("Interests")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        private void CreateUserInterestsTable()
        {
            Create.Table("UserInterests")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserName").AsString(100).ForeignKey("Users", "UserName").OnDelete(System.Data.Rule.Cascade).Indexed().NotNullable()
                .WithColumn("InterestId").AsInt32().ForeignKey("Interests", "Id").Indexed().NotNullable();
        }

        private void CreateStoriesTable()
        {
            Create.Table("Stories")
                .WithColumn("Id").AsInt32().PrimaryKey().ForeignKey("Imagebles", "Id").OnDelete(System.Data.Rule.Cascade).Indexed()
                .WithColumn("UserName").AsString(100).ForeignKey("Users", "UserName").OnDelete(System.Data.Rule.Cascade).Indexed().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Summary").AsString().NotNullable()
                .WithColumn("IsApproved").AsBoolean().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        private void CreateCommentsTable()
        {
            Create.Table("Comments")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("StoryId").AsInt32().ForeignKey("Stories", "Id").OnDelete(System.Data.Rule.Cascade).Indexed().NotNullable()
                .WithColumn("UserName").AsString(100).ForeignKey("Users", "UserName").Indexed().NotNullable()
                .WithColumn("Text").AsString().NotNullable()
                .WithColumn("IsApproved").AsBoolean().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        private void CreateRatingsTable()
        {
            Create.Table("Ratings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("StoryId").AsInt32().ForeignKey("Stories", "Id").OnDelete(System.Data.Rule.Cascade).Indexed().NotNullable()
                .WithColumn("UserName").AsString(100).ForeignKey("Users", "UserName").Indexed().NotNullable()
                .WithColumn("Rate").AsInt32().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }
    }
}
