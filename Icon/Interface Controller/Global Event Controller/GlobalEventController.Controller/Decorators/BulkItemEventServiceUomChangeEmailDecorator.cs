using GlobalEventController.Common;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.DataServices;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Controller.Decorators
{
    public class BulkItemEventServiceUomChangeEmailDecorator : IBulkEventService
    {
        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public List<ValidatedItemModel> ValidatedItemList { get; set; }
        public List<string> ScanCodesWithNoTaxList { get; set; }
        public List<NutriFactsModel> ItemNutriFacts { get; set; }
        public List<RegionalItemMessageModel> RegionalItemMessage { get; set; }

        public IBulkEventService bulkItemEventService;
        private IEmailUomChangeService emailUomChangeService;
        private IQueryHandler<GetItemsByScanCodeQuery, List<IrmaItemModel>> getItemsQueryHandler;
        private IGlobalControllerSettings settings;

        public BulkItemEventServiceUomChangeEmailDecorator(IEmailUomChangeService emailUomChangeService,
            IQueryHandler<GetItemsByScanCodeQuery, List<IrmaItemModel>> getItemsQueryHandler,
            IGlobalControllerSettings settings,
            IBulkEventService bulkItemEventService)
        {
            this.bulkItemEventService = bulkItemEventService;
            this.emailUomChangeService = emailUomChangeService;
            this.getItemsQueryHandler = getItemsQueryHandler;
            this.settings = settings;
        }

        public void Run()
        {
            this.ValidatedItemList = ValidatedItemList.DistinctBy(v => v.ScanCode).ToList();
            this.bulkItemEventService.Region = this.Region;
            this.bulkItemEventService.ItemNutriFacts = this.ItemNutriFacts;
            this.bulkItemEventService.Message = this.Message;
            this.bulkItemEventService.ReferenceId = this.ReferenceId;
            this.bulkItemEventService.ScanCodesWithNoTaxList = this.ScanCodesWithNoTaxList;
            this.bulkItemEventService.RegionalItemMessage = this.RegionalItemMessage;
            this.bulkItemEventService.ValidatedItemList = this.ValidatedItemList;

            // Check app.config to see if we need to send email alerts for retail uom changes
            if (!this.settings.SendRetailUomChangeEmailAlerts)
            {
                this.bulkItemEventService.Run();
            }
            else
            {
                // Get Items before they are updated in IRMA
                GetItemsByScanCodeQuery getItemsByScanCodeParameters = new GetItemsByScanCodeQuery { ScanCodes = ValidatedItemList.Select(v => v.ScanCode).Distinct() };
                List<IrmaItemModel> irmaItems = getItemsQueryHandler.Handle(getItemsByScanCodeParameters);

                // Run Bulk Item Event Service to update Items
                this.bulkItemEventService.Run();

                // Send Email Notifications for any UOM changes
                this.emailUomChangeService.NotifyUomChanges(irmaItems, ValidatedItemList, Region, settings.EmailSubjectEnvironment );
            }

            this.ScanCodesWithNoTaxList = this.bulkItemEventService.ScanCodesWithNoTaxList;
            this.RegionalItemMessage = this.bulkItemEventService.RegionalItemMessage;
            this.ValidatedItemList = this.bulkItemEventService.ValidatedItemList;
            this.ItemNutriFacts = this.bulkItemEventService.ItemNutriFacts;
        }
    }
}
