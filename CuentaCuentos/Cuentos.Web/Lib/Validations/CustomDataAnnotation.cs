using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuentos.Lib.Validations
{
    public class CustomDataAnnotation
    {
        public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable
        {
            public override bool IsValid(object value)
            {
                return value is bool && (bool)value;
            }

            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
            {
                return new ModelClientValidationRule[] { new ModelClientValidationRule() { ValidationType = "brequired", ErrorMessage = this.ErrorMessage } };
            }
        }
    }
}