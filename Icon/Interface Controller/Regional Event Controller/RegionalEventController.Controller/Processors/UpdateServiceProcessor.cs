using System.Collections.Generic;
using RegionalEventController.Controller.UpdateServices;
using RegionalEventController.DataAccess.Models;
using System;

namespace RegionalEventController.Controller.Processors
{
    public class UpdateServiceProcessor : INewItemProcessor
    {
        private IUpdateService updateService;

        public UpdateServiceProcessor(IUpdateService updateService)
        {
            this.updateService = updateService;
        }

        public void Run()
        {
            try
            {
                updateService.UpdateBulk();
            }
            catch (Exception)
            {
                updateService.UpdateRowByRow();
            }
        }
    }
}
