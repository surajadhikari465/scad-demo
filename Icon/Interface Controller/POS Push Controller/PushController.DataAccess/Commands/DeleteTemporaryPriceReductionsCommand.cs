using PushController.Common.Models;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class DeleteTemporaryPriceReductionsCommand
    {
        public List<TemporaryPriceReductionModel> TemporaryPriceReductions { get; set; }
    }
}
