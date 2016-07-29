using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1416346492)]
    public class AddViewColumnToStories : Migration
    {
        public override void Up()
        {
            Alter.Table("Stories").AddColumn("Views").AsInt32().WithDefaultValue(0).NotNullable();
        }

        public override void Down()
        {
            Delete.Column("Views").FromTable("Stories");

        }
    }
}
