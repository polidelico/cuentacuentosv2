using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1417457013)]
    public class AddContactsTable : Migration
    {
        
        public override void Up()
        {
            Create.Table("Contacts")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("From").AsString().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Phone").AsString().NotNullable()
                .WithColumn("SchoolId").AsInt32().ForeignKey("Schools", "Id").Indexed().Nullable()
                .WithColumn("Subject").AsString().NotNullable()
                .WithColumn("Comments").AsString(10000).NotNullable()
                .WithColumn("IsRead").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("Contacts");
        }
    }
}
