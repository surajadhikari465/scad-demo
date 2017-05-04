using Icon.RenewableContext;
using Icon.Framework;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public interface IControllerBuilder
    {
        ApiControllerBase ComposeController();
    }
}
