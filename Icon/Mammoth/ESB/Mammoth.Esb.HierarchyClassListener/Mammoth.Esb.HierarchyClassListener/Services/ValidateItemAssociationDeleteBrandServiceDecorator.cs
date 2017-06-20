using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.HierarchyClassListener.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class ValidateItemAssociationDeleteBrandServiceDecorator : IHierarchyClassService<DeleteBrandRequest>
    {
        private IHierarchyClassService<DeleteBrandRequest> deleteBrandService;
        private IQueryHandler<GetItemsByBrandIdQuery, IEnumerable<Item>> getItemsByBrandQueryHandler;
        private ListenerApplicationSettings settings;
        private IEmailClient emailClient;
        private ILogger<MammothHierarchyClassListener> logger;

        public ValidateItemAssociationDeleteBrandServiceDecorator(
            IHierarchyClassService<DeleteBrandRequest> deleteBrandService,
            IQueryHandler<GetItemsByBrandIdQuery, IEnumerable<Item>> getItemsByBrandQueryHandler,
            ListenerApplicationSettings settings,
            IEmailClient emailClient,
            ILogger<MammothHierarchyClassListener> logger)
        {
            this.deleteBrandService = deleteBrandService;
            this.getItemsByBrandQueryHandler = getItemsByBrandQueryHandler;
            this.settings = settings;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        /// <summary>
        /// This decorator will verify if there are any items associated to the brands that are in the delete brand message
        /// </summary>
        /// <param name="request">The DeleteBrandRequest object</param>
        public void ProcessHierarchyClasses(DeleteBrandRequest request)
        {
            var brands = request.HierarchyClasses
                .Where(hc => hc.HierarchyId == Hierarchies.Brands
                    && hc.Action == ActionEnum.Delete)
                .ToList();

            GetItemsByBrandIdQuery queryParameters = new GetItemsByBrandIdQuery { BrandIds = brands.Select(b => b.HierarchyClassId).ToList() };
            var items = this.getItemsByBrandQueryHandler.Search(queryParameters);

            // send email alert with items and brand being deleted
            // also remove brands from delete service request
            if (items.Any())
            {
                var brandNamesToItems = items
                    .Join(brands, ia => ia.BrandHCID, b => b.HierarchyClassId, (ia, b) => new
                    {
                        BrandId = ia.BrandHCID.GetValueOrDefault(defaultValue: 0),
                        BrandName = b.HierarchyClassName,
                        ScanCode = ia.ScanCode
                    })
                    .GroupBy(iab => iab.BrandName, iab => iab.ScanCode)
                    .ToDictionary(g => g.Key, g => g.ToList());

                try
                {
                    LogAndNotifyEmailAlert(brandNamesToItems);
                }
                catch (Exception e)
                {
                    logger.Error($"There was an exception when trying to send an email for invalid brand deletes. Exception Message:{e.Message}|" +
                        $"Inner Exception{e.InnerException?.ToString()}");
                }
                finally
                {
                    // remove invalid brands from original DeleteBrandRequest object
                    request.HierarchyClasses.RemoveAll(hc => items.Select(iab => iab.BrandHCID).Contains(hc.HierarchyClassId));
                }
            }

            // Execute DeleteBrandService
            this.deleteBrandService.ProcessHierarchyClasses(request);
        }

        private void LogAndNotifyEmailAlert(Dictionary<string, List<string>> invalidItemBrandAssociations)
        {
            logger.Error($"The following brands have items associated to them and will not be deleted: " +
                    $"{string.Join(",", invalidItemBrandAssociations.Keys)}");

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("The following Brands cannot be deleted because they are still associated to items.")
                    .Append("<br /><br />");

            foreach (KeyValuePair<string, List<string>> entry in invalidItemBrandAssociations)
            {
                builder
                    .AppendLine($"<b>{entry.Key}:</b>")
                    .Append("<br />")
                    .AppendLine(string.Join("<br />", entry.Value));
            }
            emailClient.Send(builder.ToString(), "Mammoth Hierarchy Class Listener: Brand Delete Errors");
        }
    }
}
