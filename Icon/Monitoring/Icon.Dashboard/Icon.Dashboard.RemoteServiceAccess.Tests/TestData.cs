using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceProcess;

namespace Icon.Dashboard.RemoteServicesAccess.Tests
{
    public static class TestData
    {
        public static List<string> expectedServiceNamesVmIconTest1 = new List<string>
        {
            "Icon.Esb.ItemMovement.WindowsService",
            "Icon.Esb.R10Listener.WindowsService",
            "Icon2VimQueueManagement",
            "IconControllerMonitor",
            "InforLoadTestService",
            "InforPriceListener",
            "Mammoth.ItemLocale.Controller$MA",
            "Mammoth.ItemLocale.Controller$MW",
            "Mammoth.ItemLocale.Controller$NA",
            "Mammoth.ItemLocale.Controller$NC",
            "Mammoth.ItemLocale.Controller$NE",
            "Mammoth.ItemLocale.Controller$PN",
            "Mammoth.ItemLocale.Controller$RM",
            "Mammoth.ItemLocale.Controller$SO",
            "Mammoth.ItemLocale.Controller$SP",
            "Mammoth.ItemLocale.Controller$SW",
            "Mammoth.Price.Controller$NA",
            "Mammoth.Price.Controller$SO",
            "MammothAPIController-ItemLocale",
            "MammothAPIController-ItemLocale2",
            "MammothAPIController-Price",
            "MammothAPIController-Price2",
            "MammothPrimeAffinityController",
            "NutriconServiceTest",
            "Mammoth.Price.Controller$MA",
            "Mammoth.Price.Controller$MW",
            "Mammoth.Price.Controller$NC",
            "Mammoth.Price.Controller$NE",
            "Mammoth.Price.Controller$PN",
            "Mammoth.Price.Controller$RM",
            "Mammoth.Price.Controller$SP",
            "Mammoth.Price.Controller$SW",
            "MammothAuditExtract",
            "Mammoth.Esb.HierarchyClassListener.Service",
            "Mammoth.Esb.LocaleListener.Service",
            "Mammoth.Esb.ProductListener.Service",
            "InforNewItemEventService",
            "InforHierarchyClassListener",
            "InforItemListener",
            "IconAPIController-Hierarchy",
            "IconAPIController-ProductSelectionGroup",
            "IconAPIController-Product",
            "IconAPIController-Price",
            "IconAPIController-Locale",
            "IconAPIController-ItemLocale",
        };
    }
}
