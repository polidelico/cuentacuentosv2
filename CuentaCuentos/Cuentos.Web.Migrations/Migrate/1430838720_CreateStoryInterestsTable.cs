using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1430838720)]
    public class CreateStoryInterestsTable : Migration
    {
        public override void Up()
        {
            Create.Table("StoryInterests")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("StoryId").AsInt32().ForeignKey("Stories", "Id").OnDelete(System.Data.Rule.Cascade).Indexed().NotNullable()
                .WithColumn("InterestId").AsInt32().ForeignKey("Interests", "Id").Indexed().NotNullable();

        }

        public override void Down()
        {

            Delete.Table("StoryInterests");
        }
    }
}
