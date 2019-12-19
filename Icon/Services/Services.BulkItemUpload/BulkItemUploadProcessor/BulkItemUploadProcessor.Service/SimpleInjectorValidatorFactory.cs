using FluentValidation;
using SimpleInjector;
using System;

namespace BulkItemUploadProcessor.Service
{
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