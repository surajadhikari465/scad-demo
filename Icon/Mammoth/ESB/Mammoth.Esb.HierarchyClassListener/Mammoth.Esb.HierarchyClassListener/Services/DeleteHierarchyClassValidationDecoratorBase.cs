using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public abstract class DeleteHierarchyClassValidationDecoratorBase : IHierarchyClassService<IHierarchyClassRequest>
    {
        private IHierarchyClassService<IHierarchyClassRequest> deleteHierarchyClassesService;
        private IQueryHandler<IGetAssociatedItemsParameter, IEnumerable<Item>> getAssociatedItemsQueryHandler;
        private ListenerApplicationSettings settings;
        private IEmailClient emailClient;
        private ILogger<MammothHierarchyClassListener> logger;
        protected abstract int HierarchyId { get; }
        protected abstract string HierarchyName { get; }
        protected abstract string HierarchyNamePluralized { get; }

        public DeleteHierarchyClassValidationDecoratorBase(
            IHierarchyClassService<IHierarchyClassRequest> deleteService,
            IQueryHandler<IGetAssociatedItemsParameter, IEnumerable<Item>> getAssociatedItemsQuery,
            ListenerApplicationSettings settings,
            IEmailClient emailClient,
            ILogger<MammothHierarchyClassListener> logger)
        {
            this.deleteHierarchyClassesService = deleteService;
            this.getAssociatedItemsQueryHandler = getAssociatedItemsQuery;
            this.settings = settings;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        protected abstract IGetAssociatedItemsParameter BuildQueryParameter(IList<int> hierarchyClassIDs);
        protected abstract Dictionary<string, List<string>> CompileHumanReadableErrorData(IEnumerable<HierarchyClassModel> requesto, IEnumerable<Item> associatedItems);
        protected abstract void RemoveInvalidDataFromRequest(IHierarchyClassRequest request, IEnumerable<Item> associatedItems);

        /// <summary>
        /// This decorator will verify if there are any items associated to the classes that are in
        /// the delete hierarchy classes message
        /// </summary>
        /// <param name="request">The DeleteHierarchyClassRequest object</param>
        public void ProcessHierarchyClasses(IHierarchyClassRequest request)
        {
            var classesToDelete = FilterRequest(request, HierarchyId, ActionEnum.Delete);          
            var hierarchyClassIDs = classesToDelete.Select(b => b.HierarchyClassId).ToList();
            var queryParameters = BuildQueryParameter(hierarchyClassIDs);
            var associatedItems = this.getAssociatedItemsQueryHandler.Search(queryParameters);

            if (associatedItems.Any())
            {
                // send email alert with items and what was being deleted
                // also remove associated hierarchy classes from the delete service request
                var invalidAssociations = CompileHumanReadableErrorData(classesToDelete, associatedItems);         

                try
                {
                    LogAndNotifyEmailAlert(invalidAssociations);
                }
                catch (Exception e)
                {
                    logger.Error($"There was an exception when trying to send an email for invalid {HierarchyName} deletes. Exception Message:{e.Message}|" +
                        $"Inner Exception{e.InnerException?.ToString()}");
                }
                finally
                {
                    RemoveInvalidDataFromRequest(request, associatedItems);
                }
            }
            
            this.deleteHierarchyClassesService.ProcessHierarchyClasses(request);
        }

        private IEnumerable<HierarchyClassModel> FilterRequest(IHierarchyClassRequest request, int hierarchyType, ActionEnum action)
        {
            var filterResults = request.HierarchyClasses
                .Where(hc => hc.HierarchyId == hierarchyType && hc.Action == action)
                .ToList();
            return filterResults;
        }

        private void LogAndNotifyEmailAlert(Dictionary<string, List<string>> invalidAssociations)
        {
            logger.Error($"The following {HierarchyNamePluralized} have items associated to them and will not be deleted: " +
                    $"{string.Join(",", invalidAssociations.Keys)}");

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"The following {HierarchyNamePluralized} cannot be deleted because they are still associated to items.")
                    .Append("<br /><br />");

            foreach (var entry in invalidAssociations)
            {
                builder
                    .AppendLine($"<b>{entry.Key}:</b>")
                    .Append("<br />")
                    .AppendLine(string.Join("<br />", entry.Value));
            }
            emailClient.Send(builder.ToString(), $"Mammoth Hierarchy Class Listener: {HierarchyName} Delete Errors");
        }
    }
}
