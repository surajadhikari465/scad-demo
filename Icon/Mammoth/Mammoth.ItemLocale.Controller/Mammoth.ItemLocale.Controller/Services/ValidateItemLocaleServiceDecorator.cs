using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.ItemLocale.Controller.Services
{
    /// <summary>
    /// Filters out any rows which do not have store specific data
    /// </summary>
    public class ValidateItemLocaleServiceDecorator : IService<ItemLocaleEventModel>
    {
        private IService<ItemLocaleEventModel> service;
        private ILogger logger;
        private ItemLocaleControllerApplicationSettings settings;

        public ValidateItemLocaleServiceDecorator(IService<ItemLocaleEventModel> service, ILogger logger, ItemLocaleControllerApplicationSettings settings)
        {
            this.service = service;
            this.logger = logger;
            this.settings = settings;
        }

        public void Process(List<ItemLocaleEventModel> data)
        {
            var invalidData = new List<ItemLocaleEventModel>();
            foreach (var row in data.Where(d => d.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate))
            {
                if (!IsValidItemLocaleRow(row))
                {
                    invalidData.Add(row);
                    row.ErrorMessage = ApplicationErrors.InvalidDataErrorCode;
                    this.logger.Info(String.Format("Region: {0}. ScanCode: {1}. BusinessUnitId: {2}. QueueId: {3}. " +
                                                    "The item being processed did not have store specific data. " +
                                                    "This row will be deleted and the data will be processed again when the following fields are populated: " +
                                                    "Authorized, CaseDiscount (IBM_Discount), or TM Discount (Discountable).", 
                                                    settings.CurrentRegion, row.ScanCode, row.BusinessUnitId, row.QueueId));
                }
            }

            data = data.Except(invalidData).ToList();
            this.service.Process(data);
        }

        private bool IsValidItemLocaleRow(ItemLocaleEventModel model)
        {
            if (model.Authorized == null)
                return false;
            if (model.CaseDiscount == null)
                return false;
            if (model.TmDiscount == null)
                return false;
            return true;
        }
    }
}
