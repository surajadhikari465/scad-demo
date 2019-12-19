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
using OOSCommon.DataContext.OOSEntitiesMockObjectSet;

namespace OOSCommon.DataContext
{
    /// <summary>
    /// The concrete mock context object that implements the context's interface.
    /// Provide an instance of this mock context class to client logic when testing, 
    /// instead of providing a functional context object.
    /// </summary>
    public partial class OOSEntitiesMock : IOOSEntities
    {
        public IObjectSet<KNOWN_OOS_DETAIL> KNOWN_OOS_DETAIL
        {
            get { return _kNOWN_OOS_DETAIL  ?? (_kNOWN_OOS_DETAIL = new MockObjectSet<KNOWN_OOS_DETAIL>()); }
        }
        private IObjectSet<KNOWN_OOS_DETAIL> _kNOWN_OOS_DETAIL;
        public IObjectSet<KNOWN_OOS_HEADER> KNOWN_OOS_HEADER
        {
            get { return _kNOWN_OOS_HEADER  ?? (_kNOWN_OOS_HEADER = new MockObjectSet<KNOWN_OOS_HEADER>()); }
        }
        private IObjectSet<KNOWN_OOS_HEADER> _kNOWN_OOS_HEADER;
        public IObjectSet<KNOWN_OOS_MAP> KNOWN_OOS_MAP
        {
            get { return _kNOWN_OOS_MAP  ?? (_kNOWN_OOS_MAP = new MockObjectSet<KNOWN_OOS_MAP>()); }
        }
        private IObjectSet<KNOWN_OOS_MAP> _kNOWN_OOS_MAP;
        public IObjectSet<REASON> REASON
        {
            get { return _rEASON  ?? (_rEASON = new MockObjectSet<REASON>()); }
        }
        private IObjectSet<REASON> _rEASON;
        public IObjectSet<REGION> REGION
        {
            get { return _rEGION  ?? (_rEGION = new MockObjectSet<REGION>()); }
        }
        private IObjectSet<REGION> _rEGION;
        public IObjectSet<REGION_MOVEMENT_QUERY> REGION_MOVEMENT_QUERY
        {
            get { return _rEGION_MOVEMENT_QUERY  ?? (_rEGION_MOVEMENT_QUERY = new MockObjectSet<REGION_MOVEMENT_QUERY>()); }
        }
        private IObjectSet<REGION_MOVEMENT_QUERY> _rEGION_MOVEMENT_QUERY;
        public IObjectSet<REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP> REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP
        {
            get { return _rEGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP  ?? (_rEGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP = new MockObjectSet<REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP>()); }
        }
        private IObjectSet<REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP> _rEGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP;
        public IObjectSet<REPORT_DETAIL> REPORT_DETAIL
        {
            get { return _rEPORT_DETAIL  ?? (_rEPORT_DETAIL = new MockObjectSet<REPORT_DETAIL>()); }
        }
        private IObjectSet<REPORT_DETAIL> _rEPORT_DETAIL;
        public IObjectSet<REPORT_DETAIL_ATTRIBUTE> REPORT_DETAIL_ATTRIBUTE
        {
            get { return _rEPORT_DETAIL_ATTRIBUTE  ?? (_rEPORT_DETAIL_ATTRIBUTE = new MockObjectSet<REPORT_DETAIL_ATTRIBUTE>()); }
        }
        private IObjectSet<REPORT_DETAIL_ATTRIBUTE> _rEPORT_DETAIL_ATTRIBUTE;
        public IObjectSet<REPORT_DETAIL_ATTRIBUTE_TYPE> REPORT_DETAIL_ATTRIBUTE_TYPE
        {
            get { return _rEPORT_DETAIL_ATTRIBUTE_TYPE  ?? (_rEPORT_DETAIL_ATTRIBUTE_TYPE = new MockObjectSet<REPORT_DETAIL_ATTRIBUTE_TYPE>()); }
        }
        private IObjectSet<REPORT_DETAIL_ATTRIBUTE_TYPE> _rEPORT_DETAIL_ATTRIBUTE_TYPE;
        public IObjectSet<REPORT_HEADER> REPORT_HEADER
        {
            get { return _rEPORT_HEADER  ?? (_rEPORT_HEADER = new MockObjectSet<REPORT_HEADER>()); }
        }
        private IObjectSet<REPORT_HEADER> _rEPORT_HEADER;
        public IObjectSet<SKUCount> SKUCount
        {
            get { return _sKUCount  ?? (_sKUCount = new MockObjectSet<SKUCount>()); }
        }
        private IObjectSet<SKUCount> _sKUCount;
        public IObjectSet<SOURCE> SOURCE
        {
            get { return _sOURCE  ?? (_sOURCE = new MockObjectSet<SOURCE>()); }
        }
        private IObjectSet<SOURCE> _sOURCE;
        public IObjectSet<STATUS> STATUS
        {
            get { return _sTATUS  ?? (_sTATUS = new MockObjectSet<STATUS>()); }
        }
        private IObjectSet<STATUS> _sTATUS;
        public IObjectSet<STORE> STORE
        {
            get { return _sTORE  ?? (_sTORE = new MockObjectSet<STORE>()); }
        }
        private IObjectSet<STORE> _sTORE;
        public IObjectSet<TEAM_Interim> TEAM_Interim
        {
            get { return _tEAM_Interim  ?? (_tEAM_Interim = new MockObjectSet<TEAM_Interim>()); }
        }
        private IObjectSet<TEAM_Interim> _tEAM_Interim;
        public IObjectSet<ProductStatu> ProductStatus
        {
            get { return _productStatus  ?? (_productStatus = new MockObjectSet<ProductStatu>()); }
        }
        private IObjectSet<ProductStatu> _productStatus;
        public IObjectSet<ScansMissingVimData> ScansMissingVimDatas
        {
            get { return _scansMissingVimDatas  ?? (_scansMissingVimDatas = new MockObjectSet<ScansMissingVimData>()); }
        }
        private IObjectSet<ScansMissingVimData> _scansMissingVimDatas;
    }
}
