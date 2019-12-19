using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OOS.Model;
using OOSCommon;
using StructureMap;

namespace OutOfStock.WebService
{
    public class ScanOutOfStock : IScanOutOfStock
    {
        private readonly IScanOutOfStockItemService _scanItemService;
        private readonly IOOSLog _logger;

        public ScanOutOfStock()
        {
            _scanItemService = NewScanService();
            _logger = GetLogService().GetLogger();
        }

        private IScanOutOfStockItemService NewScanService()
        {
            return ObjectFactory.GetInstance<ScanOutOfStockItemService>();
        }

        private ILogService GetLogService()
        {
            return ObjectFactory.GetInstance<ILogService>();
        }

        public string[] Validate(string[] upcs)
        {
            return _scanItemService.Validate(upcs);
        }

        public string CreateEventsFor(string storeName, string[] upcs)
        {
            DateTime scanDate = DateTime.Now;
            var result = _scanItemService.CreateEventsFor(storeName, upcs, scanDate).ToString();
            return true.ToString();
        }

        public void ScanProducts(DateTime scanDate, string regionName, string storeName, string[] upcs)
        {
            try
            {
                _scanItemService.ScanProducts(regionName, storeName, upcs, scanDate);
            }
            catch(InvalidStoreException invalidStoreException)
            {
                throw new FaultException<ValidationFaultException>(new ValidationFaultException(invalidStoreException.Message));
            }
            catch (InvalidScanDateException invalidScanDateException)
            {
                throw new FaultException<ValidationFaultException>(new ValidationFaultException(invalidScanDateException.Message));
            }
            catch (MovementDataReadException movementDataReadException)
            {
                throw new FaultException<MovementDataReadFaultException>(new MovementDataReadFaultException(movementDataReadException.Message));
            }
            catch (ProductDataReadException productDataReadException)
            {
                throw new FaultException<ProductDataReadFaultException>(new ProductDataReadFaultException(productDataReadException.Message));
            }
            catch(NoProductDataForAnyScanException noProductDataForAnyScanException)
            {
                throw new FaultException<NoProductDataForAnyScanFaultException>(new NoProductDataForAnyScanFaultException(noProductDataForAnyScanException.Message));
            }
            catch (Exception exception)
            {
                var msg = $"ScanProducts(): Failed, Message='{exception.Message}', StackTrace={exception.StackTrace}";
                _logger.Error(msg);
                throw new FaultException<ScanProductFaultException>(new ScanProductFaultException(exception.Message));
            }
        }

        public string ValidateFor(string upc)
        {
            return _scanItemService.Validate(upc).ToString();
        }

        public string CreateEventFor(string storeName, string upc)
        {
            DateTime scanDate = DateTime.Now;
            var result = _scanItemService.CreateEventFor(storeName, upc, scanDate).ToString();
            return true.ToString();
        }

        public string Ping(string echo)
        {
            return echo;
        }

        public string[] RegionNames()
        {
            return _scanItemService.RegionNames();
        }

        public string[] StoreNamesFor(string regionName)
        {
            return _scanItemService.StoreNamesFor(regionName);
        }

        public string[] RegionAbbreviations()
        {
            return _scanItemService.RegionAbbreviations();
        }

        public string[] StoreAbbreviationsFor(string regionAbbrev)
        {
            return _scanItemService.StoreAbbreviationsFor(regionAbbrev);
        }

        public void ScanProductsByStoreAbbreviation(DateTime scanDate, string regionAbbrev, string storeAbbrev, string[] upcs)
        {
            try
            {
                // scandate comes from the gun. and is unreliable. we want to use current datetime (central)
                // use currentDate instead of scanDate.
                var currentDate = DateTime.Now;
                _logger.Info($"{regionAbbrev} {storeAbbrev} {currentDate} {string.Join(",", storeAbbrev)}");
                _scanItemService.ScanProductsByStoreAbbreviation(regionAbbrev, storeAbbrev, upcs, currentDate);
            }
            catch (InvalidStoreException invalidStoreException)
            {
                _logger.Error(invalidStoreException.Message + LogMessage(regionAbbrev, storeAbbrev, scanDate, upcs));
                throw new FaultException<ValidationFaultException>(new ValidationFaultException(invalidStoreException.Message));
            }
            catch(InvalidScanDateException invalidScanDateException)
            {
                throw new FaultException<ValidationFaultException>(new ValidationFaultException(invalidScanDateException.Message));
            }
            catch (MovementDataReadException movementDataReadException)
            {
                throw new FaultException<MovementDataReadFaultException>(new MovementDataReadFaultException(movementDataReadException.Message));
            }
            catch (ProductDataReadException productDataReadException)
            {
                throw new FaultException<ProductDataReadFaultException>(new ProductDataReadFaultException(productDataReadException.Message));
            }
            catch (NoProductDataForAnyScanException noProductDataForAnyScanException)
            {
                throw new FaultException<NoProductDataForAnyScanFaultException>(new NoProductDataForAnyScanFaultException(noProductDataForAnyScanException.Message));
            }
            catch (Exception exception)
            {
                var msg = $"ScanProducts(): Failed, Message='{exception.Message}', StackTrace={exception.StackTrace}";
                _logger.Error(msg);
                throw new FaultException<ScanProductFaultException>(new ScanProductFaultException(exception.Message));
            }

        }

        private string LogMessage(string region, string store, DateTime date, IEnumerable<string> upcs)
        {
            var header = $"\r\nRegion='{region}' Store='{store}' Date='{date}'";
            var builder = new StringBuilder(header).Append("\r\n");
            upcs.ToList().ForEach(p => builder.Append(p).Append("\r\n"));
            return builder.ToString();
        }
    }
}
