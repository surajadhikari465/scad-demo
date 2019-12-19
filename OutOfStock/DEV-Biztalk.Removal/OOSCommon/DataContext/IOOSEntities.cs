//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// Architectural overview and usage guide: 
// http://blogofrab.blogspot.com/2010/08/maintenance-free-mocking-for-unit.html
//------------------------------------------------------------------------------
using System;
using System.Data.EntityClient;
using System.Data.Objects;

namespace OOSCommon.DataContext
{
    /// <summary>
    /// The interface for the specialised object context. This contains all of
    /// the <code>ObjectSet</code> properties that are implemented in both the
    /// functional context class and the mock context class.
    /// </summary>
    public interface IOOSEntities
    {
        IObjectSet<KNOWN_OOS_DETAIL> KNOWN_OOS_DETAIL { get; }
        IObjectSet<KNOWN_OOS_HEADER> KNOWN_OOS_HEADER { get; }
        IObjectSet<KNOWN_OOS_MAP> KNOWN_OOS_MAP { get; }
        IObjectSet<REASON> REASON { get; }
        IObjectSet<REGION> REGION { get; }
        IObjectSet<REGION_MOVEMENT_QUERY> REGION_MOVEMENT_QUERY { get; }
        IObjectSet<REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP> REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP { get; }
        IObjectSet<REPORT_DETAIL> REPORT_DETAIL { get; }
        IObjectSet<REPORT_DETAIL_ATTRIBUTE> REPORT_DETAIL_ATTRIBUTE { get; }
        IObjectSet<REPORT_DETAIL_ATTRIBUTE_TYPE> REPORT_DETAIL_ATTRIBUTE_TYPE { get; }
        IObjectSet<REPORT_HEADER> REPORT_HEADER { get; }
        IObjectSet<SKUCount> SKUCount { get; }
        IObjectSet<SOURCE> SOURCE { get; }
        IObjectSet<STATUS> STATUS { get; }
        IObjectSet<TEAM_Interim> TEAM_Interim { get; }
        IObjectSet<ProductStatu> ProductStatus { get; }
        IObjectSet<ScansMissingVimData> ScansMissingVimDatas { get; }
        IObjectSet<ApplicationConfig> ApplicationConfig { get; }
        IObjectSet<STORE> STORE { get; }
        IObjectSet<OutOfStockNofitication> OutOfStockNofitication { get; }
        IObjectSet<Users> Users { get; }
        IObjectSet<RegionalAppConfiguration> RegionalAppConfiguration { get; }
    }
}
