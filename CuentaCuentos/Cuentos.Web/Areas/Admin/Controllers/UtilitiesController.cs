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

namespace Cuentos.Areas.Admin.Controllers
{
    [Authorize(Roles = "superAdmin")]
    public class UtilitiesController : AdminGlobalController
    {

        public ActionResult ImportUsers()
        {
            var schools = Db.Schools.ToList().OrderBy(s => s.Name);
            ViewBag.Schools = new SelectList(schools, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult ImportUsers(int id, HttpPostedFileBase postedFile)
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

                        var createStatus = AccountController.RegisterUser(user, importedUser.Username + dobCustom, Role.RoleType.student);

                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            var createdUser = Db.Users.Find(user.UserName);
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

            var schools = Db.Schools.ToList().OrderBy(s => s.Name);
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

        public Grade getGrade(string grade)
        {
            Grade result = new Grade();
            switch (grade)
            {
                case "1":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "primero").FirstOrDefault();
                    break;
                case "2":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "segundo").FirstOrDefault();
                    break;
                case "3":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "tercero").FirstOrDefault();
                    break;
                case "4":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "cuerto").FirstOrDefault();
                    break;
                case "5":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "quinto").FirstOrDefault();
                    break;
                case "6":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "sexto").FirstOrDefault();
                    break;
                case "7":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "septimo").FirstOrDefault();
                    break;
                case "8":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "octavo").FirstOrDefault();
                    break;
                case "9":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "noveno").FirstOrDefault();
                    break;
                case "10":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "décimo").FirstOrDefault();
                    break;
                case "11":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "undécimo").FirstOrDefault();
                    break;
                case "12":
                    result = Db.Grades.Where(g => g.Name.ToLower() == "duodécimo").FirstOrDefault();
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}
