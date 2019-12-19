using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;
using NLog.Internal;
using OOS.Model;
using OOSCommon;
using OutOfStock.ScanManagement;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace OutOfStock.ScanProcessor
{
    public class ScanProcessorJob :IJob
    {
        private readonly IRawScanRepository _rawScanRepository;
        private IOOSLog _oosLogService;
        readonly Timer _timer;
        private double _initialDelay;
        private double _normalDelay;

        public ScanProcessorJob(IRawScanRepository rawScanRepository, ILogService logging)
        {
            try
            {
                _initialDelay = double.Parse(ConfigurationManager.AppSettings["InitialDelay"]);
                _normalDelay = double.Parse(ConfigurationManager.AppSettings["NormalDelay"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to load timer delay settings from App.config", ex);
            }

            _oosLogService = logging.GetLogger();
            _rawScanRepository = rawScanRepository;
            _timer = new Timer(_initialDelay) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;

        }
        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }

        private void TimerElapsed(object sender, EventArgs eventArgs)
        {
            List<RawScanData> data;
            bool KeepLooping = true;


            _timer.Enabled = false;

            if (_timer.Interval == _initialDelay) _timer.Interval = _normalDelay;
            try
            {

                while (KeepLooping)
                {
                    data = _rawScanRepository.GetNextScans(3).ToList();
                    if (!data.Any())
                    {
                        KeepLooping = false;
                    }
                    else
                    {
                        _oosLogService.Info("Processing...");
                        foreach (var scan in data)
                        {
                            try
                            {
                                var scanData = JsonConvert.DeserializeObject<ScanData>(scan.Message);
                                var elapsed = _rawScanRepository.ProcessRawScan(scanData);

                                _rawScanRepository.SetScanAsComplete(scan.Id, elapsed);
                            }
                            catch (Exception ex)
                            {
                                _rawScanRepository.SetScansAsFailed(new[] {scan.Id});

                                _oosLogService.Warn(ex.Message);
                                if (ex.InnerException != null)
                                    _oosLogService.Warn(ex.InnerException.Message);


                            }
                        }
                    }

                }

            }
            catch(Exception ex)
            {
                _oosLogService.Error(ex.Message);
                if (ex.InnerException !=null )
                       _oosLogService.Error(ex.InnerException.Message);
            }
            finally
            {
                _timer.Enabled = true;
            }

        }
    }
}