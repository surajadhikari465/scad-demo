using PushController.Common;
using PushController.Common.Exceptions;
using PushController.Common.Models;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PushController.Controller.CacheHelpers
{
    public class ScanCodeCacheHelper : ICacheHelper<string, ScanCodeModel>
    {
        private IQueryHandler<GetScanCodesByIdentifierBulkQuery, List<ScanCodeModel>> getScanCodesBulkQueryHandler;

        public ScanCodeCacheHelper(IQueryHandler<GetScanCodesByIdentifierBulkQuery, List<ScanCodeModel>> getScanCodesBulkQueryHandler)
        {
            this.getScanCodesBulkQueryHandler = getScanCodesBulkQueryHandler;
        }

        public void Populate(List<string> scanCodesToCache)
        {
            var cachedScanCodes = Cache.identifierToScanCode.Keys.ToList();
            var nonCachedScanCodes = scanCodesToCache.Except(cachedScanCodes).ToList();

            if (nonCachedScanCodes.Count > 0)
            {
                var query = new GetScanCodesByIdentifierBulkQuery
                {
                    Identifiers = nonCachedScanCodes
                };

                var scanCodes = getScanCodesBulkQueryHandler.Execute(query);

                foreach (var scanCode in scanCodes)
                {
                    Cache.identifierToScanCode.Add(scanCode.ScanCode, scanCode);
                }
            }
        }

        public ScanCodeModel Retrieve(string identifier)
        {
            ScanCodeModel scanCode;
            if (Cache.identifierToScanCode.TryGetValue(identifier, out scanCode))
            {
                return scanCode;
            }
            else
            {
                throw new ScanCodeNotFoundException(String.Format("Scan code or linked identifier {0} was not found in Icon.", identifier));
            }
        }
    }
}
