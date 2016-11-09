using GlobalEventController.Common;
using System.Collections.Generic;

namespace GlobalEventController.DataAccess.DataServices
{
    public interface IEmailUomChangeService
    {
        void NotifyUomChanges(List<IrmaItemModel> irmaItems, List<ValidatedItemModel> validatedItems, string regionAbbreviation, string emailSubjectEnvironment);
    }
}