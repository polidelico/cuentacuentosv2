using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1416335319)]
    public class AddTablesStoryCategoriesStoryGrades : Migration
    {
        public override void Up()
        {
            Create.Table("StoryCategories")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("StoryId").AsInt32().ForeignKey("Stories", "Id").OnDelete(System.Data.Rule.Cascade).Indexed().NotNullable()
                .WithColumn("CategoryId").AsInt32().ForeignKey("Categories", "Id").Indexed().NotNullable();

            Create.Table("StoryGrades")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("StoryId").AsInt32().ForeignKey("Stories", "Id").OnDelete(System.Data.Rule.Cascade).Indexed().NotNullable()
                .WithColumn("GradeId").AsInt32().ForeignKey("Grades", "Id").Indexed().NotNullable();

            Delete.ForeignKey("FK_Stories_CategoryId_Categories_Id").OnTable("Stories");
            Delete.Index("IX_Stories_CategoryId").OnTable("Stories");
            Delete.Column("CategoryId").FromTable("Stories");
        }

        public override void Down()
        {
            Delete.Table("StoryCategories");
            Delete.Table("StoryGrades");

            Alter.Table("Stories")
                .AddColumn("CategoryId").AsInt32().ForeignKey("Categories", "Id").Indexed().Nullable();

        }
    }
}
