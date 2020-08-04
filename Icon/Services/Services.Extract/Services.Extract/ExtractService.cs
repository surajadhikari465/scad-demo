﻿using System;
using Icon.Logging;

namespace Services.Extract
{
    public class ExtractService
    {
        private readonly ExtractServiceListener listenerApplication;
        private ILogger<ExtractService> logger;

        public ExtractService(ExtractServiceListener listenerApplication, ILogger<ExtractService> logger)
        {
            this.listenerApplication = listenerApplication;
            this.logger = logger;
        }

        public void Start()
        {
            listenerApplication.Run();
        }

        public void Stop()
        {
            try
            {
                listenerApplication.Close();
            }
            catch (TIBCO.EMS.IllegalStateException tibcoEx)
            {
                logger.Warn($"tibco: {tibcoEx.Message}");
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message);
            }
        }
    }
}