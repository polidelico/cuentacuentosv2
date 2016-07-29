using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentos.Web.Migrations.Migrate
{
    [Migration(1412870778)]
    public class Seed : Migration
    {
        public override void Up()
        {
            this.FillCities();
            this.FillGrades();
            //this.FillSchools();
            this.FillInterests();
            this.FillUsersAndRoles();
        }

        public override void Down()
        {
            Delete.FromTable("UserInterests").AllRows();
            Delete.FromTable("Interests").AllRows();
            Delete.FromTable("RoleMemberships").AllRows();
            Delete.FromTable("Roles").AllRows();
            Delete.FromTable("Users").AllRows();
            Delete.FromTable("Schools").AllRows();
            Delete.FromTable("Cities").AllRows();
            Delete.FromTable("Grades").AllRows();
        }

        private void FillCities()
        {
            Insert.IntoTable("Cities").Row(new { Name = "Adjuntas" });
            Insert.IntoTable("Cities").Row(new { Name = "Aguada" });
            Insert.IntoTable("Cities").Row(new { Name = "Aguadilla" });
            Insert.IntoTable("Cities").Row(new { Name = "Aguas Buenas" });
            Insert.IntoTable("Cities").Row(new { Name = "Aibonito" });
            Insert.IntoTable("Cities").Row(new { Name = "Arecibo" });
            Insert.IntoTable("Cities").Row(new { Name = "Arroyo" });
            Insert.IntoTable("Cities").Row(new { Name = "Añasco" });
            Insert.IntoTable("Cities").Row(new { Name = "Barceloneta" });
            Insert.IntoTable("Cities").Row(new { Name = "Barranquitas" });
            Insert.IntoTable("Cities").Row(new { Name = "Bayamón" });
            Insert.IntoTable("Cities").Row(new { Name = "Cabo Rojo" });
            Insert.IntoTable("Cities").Row(new { Name = "Caguas" });
            Insert.IntoTable("Cities").Row(new { Name = "Camuy" });
            Insert.IntoTable("Cities").Row(new { Name = "Canóvanas" });
            Insert.IntoTable("Cities").Row(new { Name = "Carolina" });
            Insert.IntoTable("Cities").Row(new { Name = "Cataño" });
            Insert.IntoTable("Cities").Row(new { Name = "Cayey" });
            Insert.IntoTable("Cities").Row(new { Name = "Ceiba" });
            Insert.IntoTable("Cities").Row(new { Name = "Ciales" });
            Insert.IntoTable("Cities").Row(new { Name = "Cidra" });
            Insert.IntoTable("Cities").Row(new { Name = "Coamo" });
            Insert.IntoTable("Cities").Row(new { Name = "Comerío" });
            Insert.IntoTable("Cities").Row(new { Name = "Corozal" });
            Insert.IntoTable("Cities").Row(new { Name = "Culebra" });
            Insert.IntoTable("Cities").Row(new { Name = "Dorado" });
            Insert.IntoTable("Cities").Row(new { Name = "Fajardo" });
            Insert.IntoTable("Cities").Row(new { Name = "Florida" });
            Insert.IntoTable("Cities").Row(new { Name = "Guayama" });
            Insert.IntoTable("Cities").Row(new { Name = "Guayanilla" });
            Insert.IntoTable("Cities").Row(new { Name = "Guaynabo" });
            Insert.IntoTable("Cities").Row(new { Name = "Gurabo" });
            Insert.IntoTable("Cities").Row(new { Name = "Guánica" });
            Insert.IntoTable("Cities").Row(new { Name = "Hatillo" });
            Insert.IntoTable("Cities").Row(new { Name = "Hormigueros" });
            Insert.IntoTable("Cities").Row(new { Name = "Humacao" });
            Insert.IntoTable("Cities").Row(new { Name = "Isabela" });
            Insert.IntoTable("Cities").Row(new { Name = "Jayuya" });
            Insert.IntoTable("Cities").Row(new { Name = "Juana Díaz" });
            Insert.IntoTable("Cities").Row(new { Name = "Juncos" });
            Insert.IntoTable("Cities").Row(new { Name = "Lajas" });
            Insert.IntoTable("Cities").Row(new { Name = "Lares" });
            Insert.IntoTable("Cities").Row(new { Name = "Las Marías" });
            Insert.IntoTable("Cities").Row(new { Name = "Las Piedras" });
            Insert.IntoTable("Cities").Row(new { Name = "Loíza" });
            Insert.IntoTable("Cities").Row(new { Name = "Luquillo" });
            Insert.IntoTable("Cities").Row(new { Name = "Manatí" });
            Insert.IntoTable("Cities").Row(new { Name = "Maricao" });
            Insert.IntoTable("Cities").Row(new { Name = "Maunabo" });
            Insert.IntoTable("Cities").Row(new { Name = "Mayagüez" });
            Insert.IntoTable("Cities").Row(new { Name = "Moca" });
            Insert.IntoTable("Cities").Row(new { Name = "Morovis" });
            Insert.IntoTable("Cities").Row(new { Name = "Naguabo" });
            Insert.IntoTable("Cities").Row(new { Name = "Naranjito" });
            Insert.IntoTable("Cities").Row(new { Name = "Orocovis" });
            Insert.IntoTable("Cities").Row(new { Name = "Patillas" });
            Insert.IntoTable("Cities").Row(new { Name = "Peñuelas" });
            Insert.IntoTable("Cities").Row(new { Name = "Ponce" });
            Insert.IntoTable("Cities").Row(new { Name = "Quebradillas" });
            Insert.IntoTable("Cities").Row(new { Name = "Rincón" });
            Insert.IntoTable("Cities").Row(new { Name = "Río Grande" });
            Insert.IntoTable("Cities").Row(new { Name = "Sabana Grande" });
            Insert.IntoTable("Cities").Row(new { Name = "Salinas" });
            Insert.IntoTable("Cities").Row(new { Name = "San Germán" });
            Insert.IntoTable("Cities").Row(new { Name = "San Juan" });
            Insert.IntoTable("Cities").Row(new { Name = "San Lorenzo" });
            Insert.IntoTable("Cities").Row(new { Name = "San Sebastián" });
            Insert.IntoTable("Cities").Row(new { Name = "Santa Isabel" });
            Insert.IntoTable("Cities").Row(new { Name = "Toa Alta" });
            Insert.IntoTable("Cities").Row(new { Name = "Toa Baja" });
            Insert.IntoTable("Cities").Row(new { Name = "Trujillo Alto" });
            Insert.IntoTable("Cities").Row(new { Name = "Utuado" });
            Insert.IntoTable("Cities").Row(new { Name = "Vega Alta" });
            Insert.IntoTable("Cities").Row(new { Name = "Vega Baja" });
            Insert.IntoTable("Cities").Row(new { Name = "Vieques" });
            Insert.IntoTable("Cities").Row(new { Name = "Villalba" });
            Insert.IntoTable("Cities").Row(new { Name = "Yabucoa" });
            Insert.IntoTable("Cities").Row(new { Name = "Yauco" });
        }

        private void FillGrades()
        {
            Insert.IntoTable("Grades").Row(new { Name = "Primero", Position = 1 });
            Insert.IntoTable("Grades").Row(new { Name = "Segundo", Position = 2 });
            Insert.IntoTable("Grades").Row(new { Name = "Tercero", Position = 3 });
            Insert.IntoTable("Grades").Row(new { Name = "Cuarto", Position = 4 });
            Insert.IntoTable("Grades").Row(new { Name = "Quinto", Position = 5 });
            Insert.IntoTable("Grades").Row(new { Name = "Sexto", Position = 6 });
            Insert.IntoTable("Grades").Row(new { Name = "Septimo", Position = 7 });
            Insert.IntoTable("Grades").Row(new { Name = "Octavo", Position = 8 });
            Insert.IntoTable("Grades").Row(new { Name = "Noveno", Position = 9 });
            Insert.IntoTable("Grades").Row(new { Name = "Décimo", Position = 10 });
            Insert.IntoTable("Grades").Row(new { Name = "Undécimo", Position = 11 });
            Insert.IntoTable("Grades").Row(new { Name = "Duodécimo", Position = 12 });
        }

        private void FillUsersAndRoles()
        {
            Insert.IntoTable("Roles").Row(new { RoleName = "superAdmin" });
            Insert.IntoTable("Roles").Row(new { RoleName = "schoolAdmin" });
            Insert.IntoTable("Roles").Row(new { RoleName = "student" });

            Execute.Sql(@"INSERT INTO USERS( UserName, Email, IsApproved, DateCreated, DateLastPasswordChange, PasswordHash, PasswordSalt )
                VALUES('admin', 'johan@digitaltree.com', 1, GETDATE(), GETDATE(), 0x76BF45DA01C710332BD44A6559C08639CE1498245DF1393FF0C6DBA10BF1A889321813EED9C15E9CA0BECB5AADA87699A79B4166A2F8B8BF13D37B86A2EBC0BD, 0x47D97ECBC8C3237939AC77906180599BC91458B0A79501C5CB969F8C4A1628A838678F077068CB72087DE010B1526153D76F01D2CFEFAA26BC63A3B8C47ECD7DB733DB4C3A2B299BA5CE8630F52D9DC63555A8131AE92B4A4D355859146918A82B4F3CECBA891664C20EE90B8654B49F3241F77EDA35D6CB2D4E598A4B7B5876)");
            Execute.Sql(@"INSERT INTO USERS( UserName, Email, IsApproved, DateCreated, DateLastPasswordChange, PasswordHash, PasswordSalt )
                VALUES('schooladmin', 'johan@digitaltree.com', 1, GETDATE(), GETDATE(), 0x76BF45DA01C710332BD44A6559C08639CE1498245DF1393FF0C6DBA10BF1A889321813EED9C15E9CA0BECB5AADA87699A79B4166A2F8B8BF13D37B86A2EBC0BD, 0x47D97ECBC8C3237939AC77906180599BC91458B0A79501C5CB969F8C4A1628A838678F077068CB72087DE010B1526153D76F01D2CFEFAA26BC63A3B8C47ECD7DB733DB4C3A2B299BA5CE8630F52D9DC63555A8131AE92B4A4D355859146918A82B4F3CECBA891664C20EE90B8654B49F3241F77EDA35D6CB2D4E598A4B7B5876)");
            Execute.Sql(@"INSERT INTO USERS( UserName, Email, IsApproved, DateCreated, DateLastPasswordChange, PasswordHash, PasswordSalt )
                VALUES('pepito', 'johan@digitaltree.com', 1, GETDATE(), GETDATE(), 0x76BF45DA01C710332BD44A6559C08639CE1498245DF1393FF0C6DBA10BF1A889321813EED9C15E9CA0BECB5AADA87699A79B4166A2F8B8BF13D37B86A2EBC0BD, 0x47D97ECBC8C3237939AC77906180599BC91458B0A79501C5CB969F8C4A1628A838678F077068CB72087DE010B1526153D76F01D2CFEFAA26BC63A3B8C47ECD7DB733DB4C3A2B299BA5CE8630F52D9DC63555A8131AE92B4A4D355859146918A82B4F3CECBA891664C20EE90B8654B49F3241F77EDA35D6CB2D4E598A4B7B5876)");

            Insert.IntoTable("RoleMemberships").Row(new { UserName = "admin", RoleName = "superAdmin" });
            Insert.IntoTable("RoleMemberships").Row(new { UserName = "schooladmin", RoleName = "schoolAdmin" });
            Insert.IntoTable("RoleMemberships").Row(new { UserName = "pepito", RoleName = "student" });
        }

        private void FillSchools()
        {
            Insert.IntoTable("Schools").Row(new
            {
                Name = "José S. Alegría",
                Details = "Escuela Pública",
                Address1 = "Dirección1",
                Address2 = "Dirección2",
                CityId = "1",
                Zip = "00646"
            });

            Insert.IntoTable("Schools").Row(new
            {
                Name = "Colegio de la Vega",
                Details = "Escuela Privada",
                Address1 = "Dirección1",
                Address2 = "Dirección2",
                CityId = "2",
                Zip = "12346"
            });
        }

        private void FillInterests()
        {
            Insert.IntoTable("Interests").Row(new { Name = "Dibujar" });
            Insert.IntoTable("Interests").Row(new { Name = "Pintar" });
            Insert.IntoTable("Interests").Row(new { Name = "Escribir" });
            Insert.IntoTable("Interests").Row(new { Name = "Deportes" });
        }
    }
}
