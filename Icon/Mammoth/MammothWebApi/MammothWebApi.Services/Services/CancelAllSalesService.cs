using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MammothWebApi.DataAccess.Models;
using Mammoth.Common.DataAccess;

namespace MammothWebApi.Service.Services
{
    public class CancelAllSalesService : IUpdateService<CancelAllSales>
    {
        private ILogger logger;
        private ICommandHandler<CancelAllSalesCommand> cancelAllSalesCommandHandler;

        public CancelAllSalesService(
            ILogger logger,
            ICommandHandler<CancelAllSalesCommand> cancelAllSalesCommandHandler)
        {
            this.logger = logger;
            this.cancelAllSalesCommandHandler = cancelAllSalesCommandHandler;
        }

        public void Handle(CancelAllSales data)
        {
            DateTime now = DateTime.Now;

            string region = data.CancelAllSalesData
                .Select(p => p.Region)
                .FirstOrDefault();

            var cancelAllSalesCommandParameters = new CancelAllSalesCommand();
            cancelAllSalesCommandParameters.Region = region;
            cancelAllSalesCommandParameters.Timestamp = now;
            cancelAllSalesCommandParameters.CancelAllSalesModelList = MapData(data.CancelAllSalesData);
            cancelAllSalesCommandParameters.MessageActionId = MessageActions.AddOrUpdate;

            this.cancelAllSalesCommandHandler.Execute(cancelAllSalesCommandParameters);
        }

        private List<CancelAllSalesModel> MapData(List<CancelAllSalesServiceModel> cancelAllSalesData)
        {
            List<CancelAllSalesModel> cancelAllSalesModelList = new List<CancelAllSalesModel>();

            foreach (CancelAllSalesServiceModel cancelAllSalesServiceModel in cancelAllSalesData)
            {
                CancelAllSalesModel cancelAllSalesModel = new CancelAllSalesModel();
                cancelAllSalesModel.BusinessUnitID = cancelAllSalesServiceModel.BusinessUnitId;
                cancelAllSalesModel.ScanCode = cancelAllSalesServiceModel.ScanCode;
                cancelAllSalesModel.EndDate = cancelAllSalesServiceModel.EndDate;
                cancelAllSalesModel.EventCreatedDate = cancelAllSalesServiceModel.EventCreatedDate;

                cancelAllSalesModelList.Add(cancelAllSalesModel);
            }
            return cancelAllSalesModelList;
        }
    }
}