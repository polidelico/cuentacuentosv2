using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1421261372)]
    public class PageTypesTableInheritsImageble : Migration
    {
        public override void Up()
        {
            Delete.Table("PageTypes");

            Create.Table("PageTypes")
                .WithColumn("Id").AsInt32().PrimaryKey().ForeignKey("Imagebles", "Id").OnDelete(System.Data.Rule.Cascade).Indexed()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Position").AsInt32();



            this.FillPageTypes();
        }

        public override void Down()
        {
            Delete.Table("PageTypes");

            Create.Table("PageTypes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Position").AsInt32();
        }

        private void FillPageTypes()
        {
            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO PageTypes(Id, Name, Description, Position, Active) VALUES(@@IDENTITY, 'ImageTopTextBottom', 'Imagen arriba, texto abajo', 0, 1) ");

            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO PageTypes(Id, Name, Description, Position, Active) VALUES(@@IDENTITY, 'TextTopImageBottom', 'Texto arriba, imagen abajo', 1, 1) ");

            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO PageTypes(Id, Name, Description, Position, Active) VALUES(@@IDENTITY, 'BigImageTextOverlayBottom', 'Imagen grande con texto', 2, 1) ");

            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO PageTypes(Id, Name, Description, Position, Active) VALUES(@@IDENTITY, 'TextOnly', 'solo texto', 3, 1) ");

            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO PageTypes(Id, Name, Description, Position, Active) VALUES(@@IDENTITY, 'BigImage', 'Solo imagen grande', 4, 1) ");

            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO PageTypes(Id, Name, Description, Position, Active) VALUES(@@IDENTITY, 'SmallImage', 'Solo imagen pequena', 5, 1) ");

        }
    }
}
