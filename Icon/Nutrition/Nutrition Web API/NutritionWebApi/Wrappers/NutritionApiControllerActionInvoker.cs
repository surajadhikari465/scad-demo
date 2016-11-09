using Icon.Common.Email;
using NutritionWebApi.Common;
using NutritionWebApi.Email;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace NutritionWebApi.Wrappers
{
    public class NutritionApiControllerActionInvoker : ApiControllerActionInvoker
    {
        public override Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var result = base.InvokeActionAsync(actionContext, cancellationToken);

            if (result.Exception != null && result.Exception.GetBaseException() != null)
            {
                SendEmail(actionContext, result.Exception.GetBaseException());

                var baseException = result.Exception.GetBaseException().Message;
                var errorMessagError = new System.Web.Http.HttpError("Exception Occured") { { "ErrorCode", 500 } };
                return Task.Run<HttpResponseMessage>(() => actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessagError));
            }

            return result;
        }

        private void SendEmail(HttpActionContext actionContext, Exception exception)
        {
            string method = actionContext.Request.Method.Method;
            string requestedUrl = actionContext.Request.RequestUri.AbsoluteUri;
            string user = actionContext.Request.RequestUri.UserInfo;
            string requestedIP = String.Empty;
            string requestedAgent = String.Empty;

            if (actionContext.Request.Properties.ContainsKey("MS_HttpContext"))
            {
                requestedIP = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                requestedAgent = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request.UserAgent + " AND " +
                    ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request.UserHostName;
            }

            string emailMessage = EmailBuilder.BuildEmailBody(user, requestedUrl, method, requestedIP, string.Format(NutritionConstants.NutritionUnhandledException, exception));
            EmailClient emailClient = EmailClient.CreateFromConfig();
            emailClient.Send(emailMessage, NutritionConstants.NutritionEmailSubjectException);
        }
    }
}