using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1418753829)]
    public class AlterStoryTableAddingStatus : Migration
    {

        public override void Up()
        {
            Alter.Table("Stories")
                .AddColumn("Status")
                .AsInt32()
                .NotNullable()
                .WithDefaultValue("0");

        }

        public override void Down()
        {
           Delete.Column("Status").FromTable("Stories");
        }
    }
}
