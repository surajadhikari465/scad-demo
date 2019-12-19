using FluentValidation;
using SimpleInjector;
using System;

namespace Icon.Web.Mvc.Validators
{
    /// <summary>
    /// Creates FluentValidation Validators by calling the SimpleInjector Container.
    /// Set up in the Global.asax file to automatically have a validator run and add to the ModelState.
    /// </summary>
    public class SimpleInjectorValidatorFactory : ValidatorFactoryBase
    {
        private Container container;

        public SimpleInjectorValidatorFactory(Container container)
        {
            this.container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return container.GetInstance(validatorType) as IValidator;
        }
    }
}