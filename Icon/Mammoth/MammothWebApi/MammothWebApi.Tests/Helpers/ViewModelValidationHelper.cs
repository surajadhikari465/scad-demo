using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace MammothWebApi.Tests.Helpers
{
    public static class ViewModelValidationHelper
    {
        /// <summary>
        /// Validates an object's properties against any DataAnnotations, using
        ///  the System.ComponentModel.DataAnnotations.Validator
        /// </summary>
        /// <typeparam name="T">type of the model to be validated</typeparam>
        /// <param name="viewModelToValidate">the model to be validated</param>
        /// <returns>true if the object is validated, false if it fails to pass its DataAnnotations</returns>
        public static bool Validate<T>(T viewModelToValidate)
        {
            return Validate<T>(viewModelToValidate);
        }

        /// <summary>
        /// Validates an object's properties against any DataAnnotations, using
        ///  the System.ComponentModel.DataAnnotations.Validator
        /// </summary>
        /// <typeparam name="T">type of the model to be validated</typeparam>
        /// <param name="viewModelToValidate">the model to be validated</param>
        /// <param name="modelErrorDictionary">an instantiated ModelStateDictionary object, to be filled with 
        ///     any model errors found by the validator</param>
        /// <returns>true if the object is validated, false if it fails to pass its DataAnnotations</returns>
        public static bool Validate<T>(T viewModelToValidate, ref ModelStateDictionary modelErrorDictionary)
        {
            return Validate<T>(viewModelToValidate, modelErrorDictionary);
        }

        private static bool Validate<T>(T viewModelToValidate, ModelStateDictionary modelErrorDictionary = null)
        {
            var isValid = false;
            if (viewModelToValidate != null)
            {
                var validationContext = new ValidationContext(viewModelToValidate, null, null);
                var validationResults = new List<ValidationResult>();
                isValid = Validator.TryValidateObject(viewModelToValidate, validationContext, validationResults, true);

                if (modelErrorDictionary != null)
                {
                    foreach (var validationResult in validationResults)
                    {
                        modelErrorDictionary.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? string.Empty, validationResult.ErrorMessage);
                    }
                }
            }
            return isValid;
        }
    }
}
