using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Common.Email;

namespace Icon.Infor.Listeners.HierarchyClass.Notifier
{
    public class HierarchyClassListenerNotifier : IHierarchyClassListenerNotifier
    {
        private IEmailClient emailClient;

        public HierarchyClassListenerNotifier(IEmailClient emailClient)
        {
            this.emailClient = emailClient;
        }

        public void NotifyOfError(IEsbMessage message, List<HierarchyClassModel> hierarchyClassModelsWithErrors)
        {
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
    }
}
