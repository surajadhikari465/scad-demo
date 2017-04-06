using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Mammoth.Common.DataAccess.CommandQuery;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Price.Decorators
{
    /// <summary>
    /// Archives the prices that have been processed by the PriceServiceHandler.
    /// </summary>
    public class ArchivePriceServiceDecorator : IService<PriceModel>
    {
        private IService<PriceModel> service;
        private ICommandHandler<ArchivePricesCommand> archiver;

        public ArchivePriceServiceDecorator(IService<PriceModel> service, ICommandHandler<ArchivePricesCommand> archiver)
        {
            this.service = service;
            this.archiver = archiver;
        }

        public void Process(IEnumerable<PriceModel> data, IEsbMessage message)
        {
            this.service.Process(data, message);

            Guid messageId;

            List<ArchivePriceModel> archivePrices = new List<ArchivePriceModel>();
            foreach (var price in data)
            {
                var archive = new ArchivePriceModel();
                archive.BusinessUnitID = price.BusinessUnitId;
                archive.ErrorCode = price.ErrorCode;
                archive.ErrorDetails = price.ErrorDetails;
                archive.EsbMessageID = Guid.TryParse(message.GetProperty("MessageID"), out messageId) ? messageId : default(Guid);
                archive.GpmID = price.GpmId;
                archive.InsertDate = DateTime.Now;
                archive.ItemID = price.ItemId;
                archive.JsonObject = JsonConvert.SerializeObject(price);
                archive.MessageAction = price.Action.ToString();
                archive.PriceType = price.PriceType.ToString();
                archive.Region = price.Region;
                archive.StartDate = price.StartDate;

                archivePrices.Add(archive);
            }

            this.archiver.Execute(new ArchivePricesCommand { Prices = archivePrices });
        }
    }
}
