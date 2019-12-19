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
    /// The functional concrete object context. This is just like the normal
    /// context that would be generated using the POCO artefact generator, 
    /// apart from the fact that this one implements an interface containing 
    /// the entity set properties and exposes <code>IObjectSet</code>
    /// instances for entity set properties.
    /// </summary>
    public partial class OOSEntities : ObjectContext, IOOSEntities 
    {
        public const string ConnectionString = "name=OOSEntities";
        public const string ContainerName = "OOSEntities";
    
        #region Constructors
    
        public OOSEntities():
            base(ConnectionString, ContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = true;
        }
    
        public OOSEntities(string connectionString):
            base(connectionString, ContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = true;
        }
    
        public OOSEntities(EntityConnection connection):
            base(connection, ContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = true;
        }
    
        #endregion
    
        #region ObjectSet Properties
    
        public IObjectSet<KNOWN_OOS_DETAIL> KNOWN_OOS_DETAIL
        {
            get { return _kNOWN_OOS_DETAIL ?? (_kNOWN_OOS_DETAIL = CreateObjectSet<KNOWN_OOS_DETAIL>("KNOWN_OOS_DETAIL")); }
        }
        private ObjectSet<KNOWN_OOS_DETAIL> _kNOWN_OOS_DETAIL;
    
        public IObjectSet<KNOWN_OOS_HEADER> KNOWN_OOS_HEADER
        {
            get { return _kNOWN_OOS_HEADER ?? (_kNOWN_OOS_HEADER = CreateObjectSet<KNOWN_OOS_HEADER>("KNOWN_OOS_HEADER")); }
        }
        private ObjectSet<KNOWN_OOS_HEADER> _kNOWN_OOS_HEADER;
    
        public IObjectSet<KNOWN_OOS_MAP> KNOWN_OOS_MAP
        {
            get { return _kNOWN_OOS_MAP ?? (_kNOWN_OOS_MAP = CreateObjectSet<KNOWN_OOS_MAP>("KNOWN_OOS_MAP")); }
        }
        private ObjectSet<KNOWN_OOS_MAP> _kNOWN_OOS_MAP;
    
        public IObjectSet<REASON> REASON
        {
            get { return _rEASON ?? (_rEASON = CreateObjectSet<REASON>("REASON")); }
        }
        private ObjectSet<REASON> _rEASON;
    
        public IObjectSet<REGION> REGION
        {
            get { return _rEGION ?? (_rEGION = CreateObjectSet<REGION>("REGION")); }
        }
        private ObjectSet<REGION> _rEGION;
    
        public IObjectSet<REGION_MOVEMENT_QUERY> REGION_MOVEMENT_QUERY
        {
            get { return _rEGION_MOVEMENT_QUERY ?? (_rEGION_MOVEMENT_QUERY = CreateObjectSet<REGION_MOVEMENT_QUERY>("REGION_MOVEMENT_QUERY")); }
        }
        private ObjectSet<REGION_MOVEMENT_QUERY> _rEGION_MOVEMENT_QUERY;
    
        public IObjectSet<REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP> REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP
        {
            get { return _rEGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP ?? (_rEGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP = CreateObjectSet<REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP>("REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP")); }
        }
        private ObjectSet<REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP> _rEGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP;
    
        public IObjectSet<REPORT_DETAIL> REPORT_DETAIL
        {
            get { return _rEPORT_DETAIL ?? (_rEPORT_DETAIL = CreateObjectSet<REPORT_DETAIL>("REPORT_DETAIL")); }
        }
        private ObjectSet<REPORT_DETAIL> _rEPORT_DETAIL;
    
        public IObjectSet<REPORT_DETAIL_ATTRIBUTE> REPORT_DETAIL_ATTRIBUTE
        {
            get { return _rEPORT_DETAIL_ATTRIBUTE ?? (_rEPORT_DETAIL_ATTRIBUTE = CreateObjectSet<REPORT_DETAIL_ATTRIBUTE>("REPORT_DETAIL_ATTRIBUTE")); }
        }
        private ObjectSet<REPORT_DETAIL_ATTRIBUTE> _rEPORT_DETAIL_ATTRIBUTE;
    
        public IObjectSet<REPORT_DETAIL_ATTRIBUTE_TYPE> REPORT_DETAIL_ATTRIBUTE_TYPE
        {
            get { return _rEPORT_DETAIL_ATTRIBUTE_TYPE ?? (_rEPORT_DETAIL_ATTRIBUTE_TYPE = CreateObjectSet<REPORT_DETAIL_ATTRIBUTE_TYPE>("REPORT_DETAIL_ATTRIBUTE_TYPE")); }
        }
        private ObjectSet<REPORT_DETAIL_ATTRIBUTE_TYPE> _rEPORT_DETAIL_ATTRIBUTE_TYPE;
    
        public IObjectSet<REPORT_HEADER> REPORT_HEADER
        {
            get { return _rEPORT_HEADER ?? (_rEPORT_HEADER = CreateObjectSet<REPORT_HEADER>("REPORT_HEADER")); }
        }
        private ObjectSet<REPORT_HEADER> _rEPORT_HEADER;
    
        public IObjectSet<SKUCount> SKUCount
        {
            get { return _sKUCount ?? (_sKUCount = CreateObjectSet<SKUCount>("SKUCount")); }
        }
        private ObjectSet<SKUCount> _sKUCount;
    
        public IObjectSet<SOURCE> SOURCE
        {
            get { return _sOURCE ?? (_sOURCE = CreateObjectSet<SOURCE>("SOURCE")); }
        }
        private ObjectSet<SOURCE> _sOURCE;
    
        public IObjectSet<STATUS> STATUS
        {
            get { return _sTATUS ?? (_sTATUS = CreateObjectSet<STATUS>("STATUS")); }
        }
        private ObjectSet<STATUS> _sTATUS;
    
        public IObjectSet<STORE> STORE
        {
            get { return _sTORE ?? (_sTORE = CreateObjectSet<STORE>("STORE")); }
        }
        private ObjectSet<STORE> _sTORE;
    
        public IObjectSet<TEAM_Interim> TEAM_Interim
        {
            get { return _tEAM_Interim ?? (_tEAM_Interim = CreateObjectSet<TEAM_Interim>("TEAM_Interim")); }
        }
        private ObjectSet<TEAM_Interim> _tEAM_Interim;
    
        public IObjectSet<ProductStatu> ProductStatus
        {
            get { return _productStatus ?? (_productStatus = CreateObjectSet<ProductStatu>("ProductStatus")); }
        }
        private ObjectSet<ProductStatu> _productStatus;
    
        public IObjectSet<ScansMissingVimData> ScansMissingVimDatas
        {
            get { return _scansMissingVimDatas ?? (_scansMissingVimDatas = CreateObjectSet<ScansMissingVimData>("ScansMissingVimDatas")); }
        }
        private ObjectSet<ScansMissingVimData> _scansMissingVimDatas;

        #endregion

        #region Function Imports
        public ObjectResult<SummaryStoreDetail_Result> SummaryStoreDetail(Nullable<int> psbu, Nullable<System.DateTime> start, Nullable<System.DateTime> end)
        {
    
            ObjectParameter psbuParameter;
    
            if (psbu.HasValue)
            {
                psbuParameter = new ObjectParameter("psbu", psbu);
            }
            else
            {
                psbuParameter = new ObjectParameter("psbu", typeof(int));
            }
    
            ObjectParameter startParameter;
    
            if (start.HasValue)
            {
                startParameter = new ObjectParameter("start", start);
            }
            else
            {
                startParameter = new ObjectParameter("start", typeof(System.DateTime));
            }
    
            ObjectParameter endParameter;
    
            if (end.HasValue)
            {
                endParameter = new ObjectParameter("end", end);
            }
            else
            {
                endParameter = new ObjectParameter("end", typeof(System.DateTime));
            }
            return base.ExecuteFunction<SummaryStoreDetail_Result>("SummaryStoreDetail", psbuParameter, startParameter, endParameter);
        }
        public ObjectResult<SummaryStoreHeaders_Result> SummaryStoreHeaders(string region, Nullable<System.DateTime> start, Nullable<System.DateTime> end)
        {
    
            ObjectParameter regionParameter;
    
            if (region != null)
            {
                regionParameter = new ObjectParameter("region", region);
            }
            else
            {
                regionParameter = new ObjectParameter("region", typeof(string));
            }
    
            ObjectParameter startParameter;
    
            if (start.HasValue)
            {
                startParameter = new ObjectParameter("start", start);
            }
            else
            {
                startParameter = new ObjectParameter("start", typeof(System.DateTime));
            }
    
            ObjectParameter endParameter;
    
            if (end.HasValue)
            {
                endParameter = new ObjectParameter("end", end);
            }
            else
            {
                endParameter = new ObjectParameter("end", typeof(System.DateTime));
            }
            return base.ExecuteFunction<SummaryStoreHeaders_Result>("SummaryStoreHeaders", regionParameter, startParameter, endParameter);
        }
        public ObjectResult<SummaryReportData_Result> SummaryReportData(Nullable<System.DateTime> start, Nullable<System.DateTime> end, string region)
        {
    
            ObjectParameter startParameter;
    
            if (start.HasValue)
            {
                startParameter = new ObjectParameter("start", start);
            }
            else
            {
                startParameter = new ObjectParameter("start", typeof(System.DateTime));
            }
    
            ObjectParameter endParameter;
    
            if (end.HasValue)
            {
                endParameter = new ObjectParameter("end", end);
            }
            else
            {
                endParameter = new ObjectParameter("end", typeof(System.DateTime));
            }
    
            ObjectParameter regionParameter;
    
            if (region != null)
            {
                regionParameter = new ObjectParameter("region", region);
            }
            else
            {
                regionParameter = new ObjectParameter("region", typeof(string));
            }
            return base.ExecuteFunction<SummaryReportData_Result>("SummaryReportData", startParameter, endParameter, regionParameter);
        }

        #endregion

    }
}
