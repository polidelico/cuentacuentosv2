using Cuentos.Areas.Admin.Controllers;
using Cuentos.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuentos.Lib.Binders
{
    public class PartialModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext,
                                           ModelBindingContext bindingContext, Type modelType)
        {
            var globalController = (IDataContext)controllerContext.Controller;

            var val = bindingContext.ValueProvider.GetValue("Id");    //this could be parameterised
            if (val != null)
            {
                var id = int.Parse(val.AttemptedValue);
                var entity = globalController.Db.Set(modelType).Find(id);
                if (entity != null) return entity;
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}