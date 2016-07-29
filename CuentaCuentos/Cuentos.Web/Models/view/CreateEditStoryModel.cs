using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Models.view
{
    public class CreateEditStoryModel
    {
        public CreateEditStoryModel()
        {
            selectedGrades = new string[] { };
            selectedCategories = new string[] { };
        }
        public Story Story { get; set; }

        public List<Page> Pages { get; set; }

        public string[] selectedGrades { get; set; }

        public string[] selectedCategories { get; set; }
    }
}