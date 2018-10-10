using Icon.RenewableContext;
using Icon.Framework;
using System;
using System.Collections.Generic;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public static class ControllerProvider
    {
        private static IDictionary<string, Func<IControllerBuilder>> controllerMap = new Dictionary<string, Func<IControllerBuilder>>(StringComparer.InvariantCultureIgnoreCase)
        {
            // l locale
            // h hierarchy
            // i item-locale
            // r price
            // p product
            // g product selection group

            {
                "l", () => new LocaleControllerBuilder()
            },
            {
                "h", () => new HierarchyControllerBuilder()
            },
            {
                "i", () => new ItemLocaleControllerBuilder()
            },
            {
                "r", () => new PriceControllerBuilder()
            },
            {
                "p", () => new ProductControllerBuilder()
            },
            {
                "g", () => new ProductSelectionGroupControllerBuilder()
            }
        };

        public static ApiControllerBase GetController(string controllerType)
        {
            try
            {
                return controllerMap[controllerType]().ComposeController();
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
