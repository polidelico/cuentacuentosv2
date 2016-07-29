using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1419000557)]
    public class AddPageTypesTable : Migration
    {
        public override void Up()
        {
            Create.Table("PageTypes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Position").AsInt32();

            this.FillPageTypes();
        }

        public override void Down()
        {
            Delete.Table("PageTypes");
        }

        private void FillPageTypes()
        {
            Insert.IntoTable("PageTypes").Row(new { Name = "BigImage", Description = "Solo imagen grande", Position = 0 });
            Insert.IntoTable("PageTypes").Row(new { Name = "SmallImage", Description = "Solo imagen pequena", Position = 1 });
            Insert.IntoTable("PageTypes").Row(new { Name = "TextOnly", Description = "Solo texto", Position = 2 });
            Insert.IntoTable("PageTypes").Row(new { Name = "ImageTopTextBottom", Description = "imagen arriba, texto abajo", Position = 3 });
            Insert.IntoTable("PageTypes").Row(new { Name = "TextTopImageBottom", Description = "texto arriba, imagen abajo", Position = 4 });
            Insert.IntoTable("PageTypes").Row(new { Name = "BigImageTextOverlayBottom", Description = "Imagen grande con texto", Position = 5 });
        }
    }
}
