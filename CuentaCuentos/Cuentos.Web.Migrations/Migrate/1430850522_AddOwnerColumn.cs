using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1430850522)]
    public class AddOwnerColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("Users").AddColumn("Owner").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Owner").FromTable("Users");
        }
    }
}
