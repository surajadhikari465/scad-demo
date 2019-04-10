﻿using System;
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

        //public static Dictionary<string, ServiceControllerStatus> expectedServiceNamesAndStatuses_VmIconTest2 = new Dictionary<string, ServiceControllerStatus>
        //{
        //    { "Icon Item Movement Listener", ServiceControllerStatus.Stopped },
        //    { "Icon R10 Listener", ServiceControllerStatus.Running },
        //    { "Icon API Controller - Hierarchy", ServiceControllerStatus.Running },
        //    { "Icon API Controller - ItemLocale", ServiceControllerStatus.Running },
        //    { "Icon API Controller - Locale", ServiceControllerStatus.Running },
        //    { "Icon API Controller - Price", ServiceControllerStatus.Running },
        //    { "Icon API Controller - Product", ServiceControllerStatus.Running },
        //    { "Icon API Controller - Product Selection Group", ServiceControllerStatus.Running },
        //    { "Infor HierarchyClass Listener", ServiceControllerStatus.Running },
        //    { "Infor Item Listener", ServiceControllerStatus.Running },
        //    { "Infor New Item Event Service", ServiceControllerStatus.Running },
        //    { "Mammoth API Controller - ItemLocale", ServiceControllerStatus.Running },
        //    { "Mammoth API Controller - ItemLocale2", ServiceControllerStatus.Running },
        //    { "Mammoth API Controller - Price", ServiceControllerStatus.Running },
        //    { "Mammoth API Controller - Price2", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: MA)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: MW)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: NA)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: NC)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: NE)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: PN)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: RM)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: SO)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: SP)", ServiceControllerStatus.Running },
        //    { "Mammoth ItemLocale Controller (Instance: SW)", ServiceControllerStatus.Running },
        //    { "Mammoth Prime Affinity Controller", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: MA)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: MW)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: NA)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: NC)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: NE)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: PN)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: RM)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: SO)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: SP)", ServiceControllerStatus.Running },
        //    { "Mammoth Price Controller (Instance: SW)", ServiceControllerStatus.Running },
        //    { "Mammoth Hierarchy Listener", ServiceControllerStatus.Running },
        //    { "Mammoth Locale Listener", ServiceControllerStatus.Running },
        //    { "Mammoth Product Listener", ServiceControllerStatus.Running },
        //};
    }
}
