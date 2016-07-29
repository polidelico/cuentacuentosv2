using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1417012531)]
    public class AddPagesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Pages")
                .WithColumn("Id").AsInt32().PrimaryKey().ForeignKey("Imagebles", "Id")
                .WithColumn("StoryId").AsInt32().ForeignKey("Stories", "Id").OnDelete(System.Data.Rule.Cascade).Indexed().NotNullable()
                .WithColumn("Type").AsInt32()
                .WithColumn("Text").AsString(800)
                .WithColumn("Position").AsInt32()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();

            Alter.Table("Stories").AddColumn("Pages").AsString(int.MaxValue).Nullable();
        }

        public override void Down()
        {
            Delete.Table("Pages");

            Delete.Column("Pages").FromTable("Stories");

        }
    }
}
