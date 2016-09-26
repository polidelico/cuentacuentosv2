namespace Cuentos.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Collections;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Web.Security;
    using System.Linq;
    using CodeFirstAltairis.Models;
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Cuentos.Models.CuentosContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Cuentos.Models.CuentosContext";
        }

        protected override void Seed(Cuentos.Models.CuentosContext context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //AddCities(context);
            //FillSchools(context);
            //FillRoles(context);
            //FillStories(context);
            FillUsers(context);
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                //Write to file
                System.IO.File.AppendAllLines(@"c:\errors.txt", outputLines);
                throw;

                // Showing it on screen
                throw new Exception(string.Join(",", outputLines.ToArray()));

            }
            
        }

        public void AddCities(Cuentos.Models.CuentosContext context)
        {
            context.Cities.AddOrUpdate(new Models.City() { Name = "Adjuntas" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Aguadda" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Aguadilla" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Aguas Buenas" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Aibonito" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Arecibo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Arroyo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "A�asco" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Barceloneta" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Barranquitas" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Bayamon" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Cabo Rojo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Caguas" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Camuy" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Can�vanas" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Carolina" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Cata�o" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Ceiba" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Ciales" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Cidra" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Coamo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Comerio" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Corozal" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Culebra" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Dorado" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Fajardo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Florida" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Guayama" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Guayanilla" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Gu�nica" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Hatillo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Hormigueros" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Humacao" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Isabela" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Jayuya" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Juana Diaz" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Juncos" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Lajas" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Lares" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Las Mar�as" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Las Piedras" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Luquillo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Manat�" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Maricao" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Maunabo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Mayag�ez" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Moca" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Morovis" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Naguabo" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Naranjito" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Orocovis" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Pe�uelas" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Ponce" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Quebradillas" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Rinc�n" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "R�o Grande" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Sabana Grande" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Santa Isabel" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Toa Alta" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Toa Baja" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Trujillo Alto" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Vega Alta" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Vega Baja" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Vieques" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Villalba" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Yabucoa" });
            context.Cities.AddOrUpdate(new Models.City() { Name = "Yauco" });

        }



        private void FillSchools(Cuentos.Models.CuentosContext context)
        {

            context.Schools.AddOrUpdate(new Models.School() { Name = "Jos� S. Alegr�a", Details = "Escuela P�blica", Address1 = "Direccion1", Address2 = "Direccion2", CityId = 1, Zip = "00646" });
            context.Schools.AddOrUpdate(new Models.School() { Name = "Colegio de la Vega", Details = "Escuela Privada", Address1 = "Direccion1", Address2 = "Direccion2", CityId = 2, Zip = "12346" });
        }

        private void FillRoles(Cuentos.Models.CuentosContext context)
        {
            context.Roles.AddOrUpdate(new Models.Role() { RoleName = "superAdmin" });
            context.Roles.AddOrUpdate(new Models.Role() { RoleName = "schoolAdmin" });
            context.Roles.AddOrUpdate(new Models.Role() { RoleName = "student" });
        }

        private void FillUsers(Cuentos.Models.CuentosContext context)
        {
            var service = new AccountMembershipService(null);
            var salt = getSalt();
            var salt64 = Convert.ToBase64String(salt);
            var user = new Models.User()
            {
                Name = "Rufo",
                UserName = "Rufo",
                Age = 27,
                DateCreated = DateTime.Now,
                Grade = Grade.Eighth,
                Featured = false,
                Email = "rafael.valle03@gmail.com",
                LastName = "valle",
                Roles = context.Roles.Where(r => r.RoleName == "superAdmin").ToArray(),
                PasswordHash = EncodePassword("tryagain1", salt64),
                PasswordSalt = salt,
                DateLastActivity = DateTime.Now,
                DateLastLogin = DateTime.Now,
                ApprovedDate = DateTime.Now,
                DateLastPasswordChange = DateTime.Now,
                IsApproved = true,
                
                Owner = "Student",

            };
            context.Users.AddOrUpdate(user);
        }
        public byte[] EncodePassword(string pass, string saltBase64)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Convert.FromBase64String(saltBase64);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return inArray;
        }
        private static byte[] getSalt()
        {
            var random = new RNGCryptoServiceProvider();

            // Maximum length of salt
            int max_length = 64;

            // Empty salt array
            byte[] salt = new byte[max_length];

            // Build the random bytes
            random.GetNonZeroBytes(salt);

            // Return the string encoded salt
            return salt;
        }

        private void FillStories(Cuentos.Models.CuentosContext context)
        {
            var story = new Models.Story(true);
           
            context.Stories.AddOrUpdate(story);
        }
    }
}
