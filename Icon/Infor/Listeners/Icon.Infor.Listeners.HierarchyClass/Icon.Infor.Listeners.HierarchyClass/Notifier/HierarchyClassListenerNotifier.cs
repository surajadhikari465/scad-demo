using System.Collections.Generic;
using System.Text;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Common.Email;
using Icon.Esb.Services.ConfirmationBod;
using Esb.Core.EsbServices;
using Icon.Infor.Listeners.HierarchyClass.Constants;

namespace Icon.Infor.Listeners.HierarchyClass.Notifier
{
    public class HierarchyClassListenerNotifier : IHierarchyClassListenerNotifier
    {
        private const string WfmTenantId = "WFM";
        private IEmailClient emailClient;
        private HierarchyClassListenerSettings settings;
        private IEsbService<ConfirmationBodEsbRequest> confirmationBodeEsbService;

        public HierarchyClassListenerNotifier(IEmailClient emailClient,
                                              HierarchyClassListenerSettings settings,
                                              IEsbService<ConfirmationBodEsbRequest> confirmationBodeEsbService
                                              )
        {
            this.emailClient = emailClient;
            this.settings = settings;
            this.confirmationBodeEsbService = confirmationBodeEsbService;
        }

        public void NotifyOfError(IEsbMessage message, ConfirmationBodEsbErrorTypes errorType, List<InforHierarchyClassModel> hierarchyClassModelsWithErrors)
        {
            if (settings.EnableConfirmBods)
            {
                SendConfirmationBod(message, errorType, hierarchyClassModelsWithErrors);
            }
            if (hierarchyClassModelsWithErrors.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("The following hierarchy classes from Infor had errors when adding or updating the hierarchies in Icon.")
                    .Append("<br /><br />");

                builder.AppendFormat("<b>Infor Message ID:</b> {0}", message.GetProperty("IconMessageID"))
                    .Append("<br /><br />");

                foreach (var hierarchyClass in hierarchyClassModelsWithErrors)
                {
                    builder
                        .AppendFormat("    <b>Hierarchy Class ID:</b> {0}", hierarchyClass.HierarchyClassId)
                        .Append("<br />")
                        .AppendFormat("    <b>Hierarchy Type:</b> {0}", hierarchyClass.HierarchyName)
                        .Append("<br />")
                        .AppendFormat("    <b>Hierarchy Class Name:</b> {0}", hierarchyClass.HierarchyClassName)
                        .Append("<br />")
                        .AppendFormat("    <b>Parent Hierarchy Class Id:</b> {0}", hierarchyClass.ParentHierarchyClassId)
                        .Append("<br />")
                        .AppendFormat("    <b>Hierarchy Level Name:</b> {0}", hierarchyClass.HierarchyLevelName)
                        .Append("<br />")
                        .AppendFormat("    <b>Action:</b> {0}", hierarchyClass.Action.ToString())
                        .Append("<br />")
                        .AppendFormat("    <b>Error Code:</b> {0}", hierarchyClass.ErrorCode)
                        .Append("<br />")
                        .AppendFormat("    <b>Error Details:</b> {0}", hierarchyClass.ErrorDetails)
                        .Append("<br /><br />");
                }
                emailClient.Send(builder.ToString(), "Infor Hierarchy Class Listener: Hierarchy Class Errors");
            }
        }

        private void SendConfirmationBod(IEsbMessage message, ConfirmationBodEsbErrorTypes errorType, List<InforHierarchyClassModel> hierarchyClassModelsWithErrors)
        {
            var messageId = message.GetProperty("IconMessageID");
            ConfirmationBodEsbRequest request;
            switch (errorType)
            {
                case ConfirmationBodEsbErrorTypes.Schema:

                    request = new ConfirmationBodEsbRequest
                    {
                        BodId = messageId,
                        ErrorDescription = ApplicationErrors.Descriptions.UnableToParseHierarchyClass,
                        ErrorReasonCode = ApplicationErrors.Codes.UnableToParseHierarchyClass,
                        ErrorType = ConfirmationBodEsbErrorTypes.Schema,
                        EsbMessageProperties = new Dictionary<string, string>
                    {
                        { "IconMessageID", messageId }
                    },
                        OriginalMessage = message.MessageText,
                        TenantId = WfmTenantId
                    };
                    confirmationBodeEsbService.Send(request);
                    break;

                case ConfirmationBodEsbErrorTypes.Data:
                case ConfirmationBodEsbErrorTypes.DatabaseConstraint:
                    foreach (var hierarchyClass in hierarchyClassModelsWithErrors)
                    {
                        request = new ConfirmationBodEsbRequest
                        {
                            BodId = messageId,
                            ErrorDescription = hierarchyClass.ErrorDetails,
                            ErrorReasonCode = hierarchyClass.ErrorCode,
                            ErrorType = errorType,
                            EsbMessageProperties = new Dictionary<string, string>
                        {
                            { "IconMessageID", messageId }
                        },
                            OriginalMessage = message.MessageText,
                            TenantId = WfmTenantId
                        };
                        confirmationBodeEsbService.Send(request);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
