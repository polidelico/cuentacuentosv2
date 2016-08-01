using CodeFirstAltairis.Controllers;
using CsvHelper;
using Cuentos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Cuentos.Lib.Extensions;
using System.Data.Entity;
using System.Threading.Tasks;
namespace Cuentos.Areas.Admin.Controllers
{
    [Authorize(Roles = "superAdmin")]
    public class UtilitiesController : AdminGlobalController
    {

        public async Task<ActionResult> ImportUsers()
        {
            var schools = await Db.Schools.OrderBy(s => s.Name).ToListAsync();
            ViewBag.Schools = new SelectList(schools, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ImportUsers(int id, HttpPostedFileBase postedFile)
        {
            if (postedFile != null && postedFile.ContentType == "application/octet-stream")
            {
                var message = "{0} usuarios importados satisfactoriamente. {1} errores";
                var successCount = 0;
                var errorCount = 0;

                using (var reader = new StreamReader(postedFile.InputStream))
                using (var csvReader = new CsvReader(reader))
                {
                    var users = csvReader.GetRecords<UserToImport>().ToList();


                    foreach (var importedUser in users)
                    {
                        var splitedName = importedUser.Name.Split(',');
                        var dobCustom = importedUser.DOB.Replace("/", "");
                        var dob = DateTime.ParseExact(importedUser.DOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        var userAge = DateTime.Now.Year - dob.Year;
                        var userGrade = getGrade(importedUser.Grade);
                        var user = new User
                        {
                            UserName = importedUser.Username,
                            Name = splitedName.Count() > 1 ? splitedName[1] : splitedName[0],
                            LastName = splitedName.Count() > 1 ? splitedName[0] : "",
                            Email = "cambiar@email.edu",
                            Age = userAge,
                            ApprovedDate = DateTime.Now,
                            ApprovedBy = LoggedUser.UserName,
                            SchoolId = id,
                            Owner = "Student"
                        };

                        if (userGrade != null)
                        {
                            user.GradeId = userGrade.Id;
                        }

                        var createStatus = await AccountController.RegisterUser(user, importedUser.Username + dobCustom, Role.RoleType.student);

                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            var createdUser = await Db.Users.FindAsync(user.UserName);
                            createdUser.IsApproved = true;
                            Db.SaveChanges();
                            successCount++;
                        }
                        else
                        {
                            errorCount++;
                        }


                    }
                }

                if (errorCount > 0)
                {
                    RedirectToAction("ImportUsers", "Utilities").Error(string.Format(message, successCount, errorCount));
                }
                else
                {
                    RedirectToAction("ImportUsers", "Utilities").Success(string.Format(message, successCount, errorCount));
                }

            }
            else
            {
                RedirectToAction("ImportUsers", "Utilities").Error("Archivo provisto no es aceptable.");
            }

            var schools = await Db.Schools.OrderBy(s => s.Name).ToListAsync();
            ViewBag.Schools = new SelectList(schools, "Id", "Name");
            return View();
        }



        public class UserToImport
        {
            public string Username { get; set; }
            public string Name { get; set; }
            public string DOB { get; set; }
            public string Grade { get; set; }
            //public string Genre { get; set; }
        }

        public async Task<Grade> getGrade(string grade)
        {
            Grade result = new Grade();
            switch (grade)
            {
                case "1":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "primero").FirstOrDefaultAsync();
                    break;
                case "2":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "segundo").FirstOrDefaultAsync();
                    break;
                case "3":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "tercero").FirstOrDefaultAsync();
                    break;
                case "4":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "cuerto").FirstOrDefaultAsync();
                    break;
                case "5":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "quinto").FirstOrDefaultAsync();
                    break;
                case "6":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "sexto").FirstOrDefaultAsync();
                    break;
                case "7":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "septimo").FirstOrDefaultAsync();
                    break;
                case "8":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "octavo").FirstOrDefaultAsync();
                    break;
                case "9":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "noveno").FirstOrDefaultAsync();
                    break;
                case "10":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "décimo").FirstOrDefaultAsync();
                    break;
                case "11":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "undécimo").FirstOrDefaultAsync();
                    break;
                case "12":
                    result = await Db.Grades.Where(g => g.Name.ToLower() == "duodécimo").FirstOrDefaultAsync();
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}
