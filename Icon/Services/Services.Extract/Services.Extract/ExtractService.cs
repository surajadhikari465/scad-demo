using System;
using Icon.Logging;

namespace Services.Extract
{
    public class ExtractService
    {

        private readonly ExtractServiceListener ListenerApplication;
        private ILogger<ExtractService> Logger;

        public ExtractService(ExtractServiceListener listenerApplication, ILogger<ExtractService> logger)
        {
            ListenerApplication = listenerApplication;
            Logger = logger;
        }

        public void Start()
        {

            ListenerApplication.Run();

        }

        public void Stop()
        {
            try
            {
                ListenerApplication.Close();
            }
            catch (TIBCO.EMS.IllegalStateException tibcoEx)
            {
                Logger.Warn($"tibco: {tibcoEx.Message}");
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }
    }
}