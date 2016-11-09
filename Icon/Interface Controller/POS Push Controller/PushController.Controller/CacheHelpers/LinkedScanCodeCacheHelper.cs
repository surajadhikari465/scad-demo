using PushController.Common;
using PushController.Common.Exceptions;
using PushController.Common.Models;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;

namespace PushController.Controller.CacheHelpers
{
    public class LinkedScanCodeCacheHelper : ICacheHelper<Tuple<string, int>, string>
    {
        private IQueryHandler<GetCurrentLinkedScanCodesQuery, List<CurrentLinkedScanCodeModel>> getCurrentLinkedScanCodesQueryHandler;

        public LinkedScanCodeCacheHelper(IQueryHandler<GetCurrentLinkedScanCodesQuery, List<CurrentLinkedScanCodeModel>> getCurrentLinkedScanCodesQueryHandler)
        {
            this.getCurrentLinkedScanCodesQueryHandler = getCurrentLinkedScanCodesQueryHandler;
        }

        public void Populate(List<Tuple<string, int>> scanCodesByBusinessUnit)
        {
            var query = new GetCurrentLinkedScanCodesQuery
            {
                ScanCodesByBusinessUnit = scanCodesByBusinessUnit
            };

            var linkedScanCodes = getCurrentLinkedScanCodesQueryHandler.Execute(query);

            foreach (var scanCode in linkedScanCodes)
            {
                Tuple<string, int> key = new Tuple<string, int>(scanCode.ScanCode, scanCode.BusinessUnitId);

                if (Cache.scanCodeByBusinessUnitToLinkedScanCode.ContainsKey(key))
                {
                    Cache.scanCodeByBusinessUnitToLinkedScanCode[key] = scanCode.LinkedScanCode;
                }
                else
                {
                    Cache.scanCodeByBusinessUnitToLinkedScanCode.Add(new Tuple<string, int>(scanCode.ScanCode, scanCode.BusinessUnitId), scanCode.LinkedScanCode);
                }
            }
        }

        public string Retrieve(Tuple<string, int> scanCodeByBusinessUnit)
        {
            string linkedScanCode;
            if (Cache.scanCodeByBusinessUnitToLinkedScanCode.TryGetValue(scanCodeByBusinessUnit, out linkedScanCode))
            {
                return linkedScanCode;
            }
            else
            {
                return null;
            }
        }
    }
}
