using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1415899231)]
    public class _1415899231_Add_Featured_Column : Migration
    {
        public override void Up()
        {
            this.AlterStoriesTable();
        }

        public override void Down()
        {
            Delete.Column("Featured").FromTable("Stories");
        }


        private void AlterStoriesTable()
        {
            Alter.Table("Stories")
                 .AddColumn("Featured").AsBoolean().NotNullable();
        }
    }
}
