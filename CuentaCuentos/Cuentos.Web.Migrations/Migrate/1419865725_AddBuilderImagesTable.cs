using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1419865725)]
    public class AddBuilderImagesTable : Migration
    {
        public override void Up()
        {
            Create.Table("BuilderGalleries")
                .WithColumn("Id").AsInt32().PrimaryKey().ForeignKey("Imagebles", "Id").OnDelete(System.Data.Rule.Cascade).Indexed()
                .WithColumn("Name").AsString().Indexed().NotNullable()
                .WithColumn("Description").AsString()
                .WithColumn("UserName").AsString(100).ForeignKey("Users", "UserName").Indexed().Nullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();

            Create.Table("ImageCategories")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().Indexed().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();

            AddData();


        }

        public override void Down()
        {
            Delete.Table("BuilderGalleries");
            Delete.Table("ImageCategories");
        }

        public void AddData()
        {
            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO BuilderGalleries(Id, Name, Description, CreatedAt) VALUES(@@IDENTITY, 'Main', 'Imagenes Generales', GETDATE()) ");

            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO BuilderGalleries(Id, Name, Description, CreatedAt) VALUES(@@IDENTITY, 'San Valentin', 'Imagenes para epoca de San Valentin', GETDATE()) ");

            Execute.Sql(@"INSERT INTO Imagebles DEFAULT VALUES
                INSERT INTO BuilderGalleries(Id, Name, Description, CreatedAt) VALUES(@@IDENTITY, 'Navidad', 'Imagenes para epoca de Navidad', GETDATE()) ");

            Insert.IntoTable("ImageCategories").Row(new { Name = "Aves", CreatedAt = DateTime.Now });
            Insert.IntoTable("ImageCategories").Row(new { Name = "Estrellas", CreatedAt = DateTime.Now });
            Insert.IntoTable("ImageCategories").Row(new { Name = "Alegria", CreatedAt = DateTime.Now });
            Insert.IntoTable("ImageCategories").Row(new { Name = "Miedo", CreatedAt = DateTime.Now });
        }
    }
}
