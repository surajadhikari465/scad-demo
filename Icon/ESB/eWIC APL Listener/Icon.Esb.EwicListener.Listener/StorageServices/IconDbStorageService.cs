using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Esb.EwicAplListener.StorageServices
{
    public class IconDbStorageService : IAplStorageService
    {
        private ILogger<IconDbStorageService> logger;
        private ICommandHandler<AddMessageHistoryParameters> addMessageHistoryCommand;
        private IQueryHandler<AgencyExistsParameters, bool> agencyExistsQuery;
        private ICommandHandler<AddAgencyParameters> addAgencyCommand;
        private ICommandHandler<AddOrUpdateAuthorizedProductListParameters> addOrUpdateAplCommand;

        public IconDbStorageService(
            ILogger<IconDbStorageService> logger,
            ICommandHandler<AddMessageHistoryParameters> addMessageHistoryCommand,
            IQueryHandler<AgencyExistsParameters, bool> agencyExistsQuery,
            ICommandHandler<AddAgencyParameters> addAgencyCommand,
            ICommandHandler<AddOrUpdateAuthorizedProductListParameters> addOrUpdateAplCommand)
        {
            this.logger = logger;
            this.addMessageHistoryCommand = addMessageHistoryCommand;
            this.agencyExistsQuery = agencyExistsQuery;
            this.addAgencyCommand = addAgencyCommand;
            this.addOrUpdateAplCommand = addOrUpdateAplCommand;
        }

        public void Save(AuthorizedProductListModel model)
        {
            var aplMessage = new MessageHistory();

            SaveMessage(aplMessage, model.MessageXml);
            InsertNewAgencies(model.Items.Select(i => i.AgencyId).Distinct().ToList());
            UpdateAuthorizedProductList(model.Items, aplMessage.MessageHistoryId);
        }

        private void UpdateAuthorizedProductList(List<EwicItemModel> items, int messageHistoryId)
        {
            logger.Info(String.Format("Preparing to update the database for APL message {0}.  There are {1} entries to process.",
                messageHistoryId, items.Count));

            foreach (var item in items)
            {
                var newAplEntry = new AuthorizedProductList
                {
                    AgencyId = item.AgencyId,
                    ScanCode = item.ScanCode,
                    ItemDescription = item.ItemDescription,
                    PackageSize = item.PackageSize,
                    UnitOfMeasure = item.UnitOfMeasure,
                    BenefitQuantity = item.BenefitQuantity,
                    BenefitUnitDescription = item.BenefitUnitDescription,
                    ItemPrice = item.ItemPrice,
                    PriceType = item.PriceType
                };

                var parameters = new AddOrUpdateAuthorizedProductListParameters { Apl = newAplEntry };

                addOrUpdateAplCommand.Execute(parameters);

                logger.Info(String.Format("Updated the APL with agency {0} and scan code {1}.", item.AgencyId, item.ScanCode));
            }
        }

        private void InsertNewAgencies(List<string> agencies)
        {
            foreach (var agencyId in agencies)
            {
                bool agencyExists = agencyExistsQuery.Search(new AgencyExistsParameters { AgencyId = agencyId });

                if (!agencyExists)
                {
                    var agency = new Agency { AgencyId = agencyId };

                    var parameters = new AddAgencyParameters { Agency = agency };

                    addAgencyCommand.Execute(parameters);

                    logger.Info(String.Format("Added agency {0} to the database.", agencyId));
                }
                else
                {
                    logger.Info(String.Format("Agency {0} already exists and will not be saved to the database.", agencyId));
                }
            }
        }

        private void SaveMessage(MessageHistory message, string messageXml)
        {
            message.MessageTypeId = MessageTypes.Ewic;
            message.MessageStatusId = MessageStatusTypes.Consumed;
            message.InsertDate = DateTime.Now;
            message.Message = messageXml;
            message.InProcessBy = null;
            message.ProcessedDate = DateTime.Now;

            var parameters = new AddMessageHistoryParameters { Message = message };

            addMessageHistoryCommand.Execute(parameters);

            logger.Info(String.Format("Saved eWIC message {0} to the database.", message.MessageHistoryId));
        }
    }
}
