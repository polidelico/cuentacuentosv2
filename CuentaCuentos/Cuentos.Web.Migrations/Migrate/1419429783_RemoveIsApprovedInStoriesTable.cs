using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1419429783)]
    public class RemoveIsApprovedInStoriesTable : Migration
    {
        public override void Up()
        {

            Delete.Column("IsApproved").FromTable("Stories");
        }

        public override void Down()
        {
            
            Alter.Table("Stories")
            .AddColumn("IsApproved")
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue("false");

        }
    }
}
