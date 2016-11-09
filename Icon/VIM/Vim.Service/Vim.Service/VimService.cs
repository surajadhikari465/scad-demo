namespace Vim.Service
{
    using Icon.Common.Email;
    using Newtonsoft.Json;
    using NLog;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Vim.Service.Models;

    public class VimService : IVimService
    {
        private IVimBrandClient brandClient;
        private Icon.Logging.ILogger<VimService> logger;
        private IEmailClient emailClient;

        public VimService()
        {
            var container = SimpleInjectorInitializer.InitializeContainer();
            this.logger = container.GetInstance<Icon.Logging.ILogger<VimService>>();
            this.brandClient = container.GetInstance<IVimBrandClient>();
            this.emailClient = container.GetInstance<IEmailClient>();
        }

        public VimService(
            IVimBrandClient client,
            Icon.Logging.ILogger<VimService> logger,
            IEmailClient emailClient)
        {
            this.logger = logger;
            this.brandClient = client;
            this.emailClient = emailClient;
        }
        
        public async Task<VimBrandResponse> SendBrandsAsync(VimBrandRequest brandRequest)
        {
            bool isSuccessful = false;

            try
            {
                // Send Brands to VIM implementation performed by the IVimBrandClient
                // Can either be an httpclient or possibly to the ESB queue in the future.
                var sendBrandsTask = this.brandClient.SendBrandsToVimAsync(brandRequest.Brands);

                // Log the request while you wait for the client to return.
                this.LogSendBrandsRequest(brandRequest);

                var response = await sendBrandsTask;
                isSuccessful = response.IsSuccessful;
                if (!isSuccessful)
                {
                    this.LogSendBrandsFailure(brandRequest, failMessage:response.FailureMessage);
                }

                return new VimBrandResponse() { IsSuccessful = isSuccessful };
            }
            catch(Exception e)
            {
                this.LogSendBrandsFailure(brandRequest, ex: e);
                return new VimBrandResponse() { IsSuccessful = false };
            }
        }

        private void LogSendBrandsRequest(VimBrandRequest brandRequest)
        {
            MappedDiagnosticsContext.Clear();
            MappedDiagnosticsContext.Set(NLogParams.Action, VimServiceActions.SendBrandsToVim);

            var brands = JsonConvert.SerializeObject(brandRequest?.Brands);

            this.logger?.Info(brands);
        }

        private void LogSendBrandsFailure(VimBrandRequest brandRequest, string failMessage = null, Exception ex = null)
        {
            MappedDiagnosticsContext.Clear();
            MappedDiagnosticsContext.Set(NLogParams.Action, VimServiceActions.SendBrandsToVim);
            MappedDiagnosticsContext.Set(NLogParams.Error, "Failure to send Brands to VIM");

            if(ex != null || !string.IsNullOrEmpty(failMessage))
            {
                MappedDiagnosticsContext.Set(NLogParams.ErrorDetails, failMessage ?? ex.Message);
            }

            var brands = JsonConvert.SerializeObject(brandRequest?.Brands);

            this.logger?.Error(brands);

            this.SendEmail(brandRequest, failMessage, ex);
        }

        private void SendEmail(VimBrandRequest brandRequest, string failMessage = null, Exception e = null)
        {
            var brands = JsonConvert.SerializeObject(brandRequest?.Brands);

            var message = new StringBuilder("VIM Service could not properly send Brand information.");
            message.AppendLine(brands);
            message.AppendLine(failMessage);
            message.AppendLine(e?.Message);
            message.AppendLine(e?.StackTrace);

            this.emailClient?.Send(message.ToString(), "VIM Service Failure");
        }
    }
}
