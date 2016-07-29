using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1415906902)]
    public class AddCategoriesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Categories")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();

            Alter.Table("Stories")
                .AddColumn("CategoryId").AsInt32().ForeignKey("Categories", "Id").Indexed().Nullable();

        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Stories_CategoryId_Categories_Id").OnTable("Stories");
            Delete.Index("IX_Stories_CategoryId").OnTable("Stories");
            Delete.Column("CategoryId").FromTable("Stories");
            Delete.Table("Categories");
        }
    }
}
