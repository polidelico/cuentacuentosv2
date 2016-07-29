using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1416263869)]
    public class AddFeaturedColumnUsers : Migration
    {
        public override void Up()
        {
            Alter.Table("Users")
                 .AddColumn("Featured").AsBoolean().WithDefaultValue(false).NotNullable();
        }

        public override void Down()
        {
            Delete.Column("Featured").FromTable("Users");
        }

    }
}
