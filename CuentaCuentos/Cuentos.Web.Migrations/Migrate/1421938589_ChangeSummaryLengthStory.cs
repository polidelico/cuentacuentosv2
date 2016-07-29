using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1421938589)]
    public class ChangeSummaryLengthStory : Migration
    {
        public override void Up()
        {

            Alter.Column("Summary").OnTable("Stories").AsString(800);

        }

        public override void Down()
        {

            Alter.Column("Summary").OnTable("Stories").AsString();

        }
    }
}
