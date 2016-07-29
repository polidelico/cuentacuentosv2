using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1416318645)]
    public class AddApprovedDateAndApprovedByColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("Stories")
                .AddColumn("ApprovedDate").AsDateTime().Nullable()
                .AddColumn("ApprovedBy").AsString(100).ForeignKey("Users", "UserName").Indexed().Nullable();

            Alter.Table("Users")
                .AddColumn("ApprovedDate").AsDateTime().Nullable()
                .AddColumn("ApprovedBy").AsString(100).ForeignKey("Users", "UserName").Indexed().Nullable();

            Alter.Table("Comments")
                .AddColumn("ApprovedDate").AsDateTime().Nullable()
                .AddColumn("ApprovedBy").AsString(100).ForeignKey("Users", "UserName").Indexed().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Stories_ApprovedBy_Users_UserName").OnTable("Stories");
            Delete.Index("IX_Stories_ApprovedBy").OnTable("Stories");
            Delete.Column("ApprovedDate")
                .Column("ApprovedBy")
                .FromTable("Stories");

            Delete.ForeignKey("FK_Users_ApprovedBy_Users_UserName").OnTable("Users");
            Delete.Index("IX_Users_ApprovedBy").OnTable("Users");
            Delete.Column("ApprovedDate")
                .Column("ApprovedBy")
                .FromTable("Users");

            Delete.ForeignKey("FK_Comments_ApprovedBy_Users_UserName").OnTable("Comments");
            Delete.Index("IX_Comments_ApprovedBy").OnTable("Comments");
            Delete.Column("ApprovedDate")
                .Column("ApprovedBy")
                .FromTable("Comments");
        }
    }
}
