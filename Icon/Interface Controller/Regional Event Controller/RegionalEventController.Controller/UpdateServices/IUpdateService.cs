using RegionalEventController.DataAccess.Models;
using System.Collections.Generic;

namespace RegionalEventController.Controller.UpdateServices
{
    public interface IUpdateService
    {
        void UpdateBulk();
        void UpdateRowByRow();
    }
}
