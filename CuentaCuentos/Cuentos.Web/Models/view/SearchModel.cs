using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Models.view
{
    public class SearchModel
    {
        public SearchModel()
        {
            selectedGrades = new string[] { };
            selectedCategories = new string[] { };
        }
        public string q { get; set; }

        //public int?[] Grades { get; set; }

        public int? CityId { get; set; }

        public int? SchoolId { get; set; }

        public List<Grade> Grades { get; set; }

        public IPagedList<Story> Stories { get; set; }

        public string[] selectedGrades { get; set; }

        public string[] selectedCategories { get; set; }


    }
}