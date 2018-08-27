using System;

namespace MammothWebApi.Service.Models
{
    public class ItemLocaleServiceModel
    {
        // 'Core' Attributes
        public string Region { get; set; }
        public int BusinessUnitId { get; set; }
        public string ScanCode { get; set; }
        public bool CaseDiscount { get; set; }
        public bool TMDiscount { get; set; }
        public int? AgeRestriction { get; set; }
        public bool RestrictedHours { get; set; }
        public bool Authorized { get; set; }
        public bool Discontinued { get; set; }
        public string LabelTypeDescription { get; set; }
        public bool LocalItem { get; set; }
        public string ProductCode { get; set; }
        public string RetailUnit { get; set; }
        public string SignDescription { get; set; }
        public string Locality { get; set; }
        public string SignRomanceLong { get; set; }
        public string SignRomanceShort { get; set; }
        public decimal Msrp { get; set; }
        public bool? OrderedByInfor { get; set; }
        public decimal? AltRetailSize { get; set; }
        public string AltRetailUOM { get; set; }
        public bool? DefaultScanCode { get; set; }
        public int? IrmaItemKey { get; set; }

        // Supplier Attributes
        public string SupplierName { get; set; }
        public string SupplierItemId { get; set; }
        public decimal? SupplierCaseSize { get; set; }
        public string IrmaVendorKey { get; set; }


        //Scale Attributes
        public bool? ForceTare { get; set; }
        public bool? SendtoCFS { get; set; }
        public string WrappedTareWeight { get; set; }
        public string UnwrappedTareWeight { get; set; }
        public bool? ScaleItem { get; set; }
        public string UseBy { get; set; }
        public int? ShelfLife { get; set; }

        // 'Extended' Attributes

        /// <summary>
        /// Extended Property
        /// </summary>
        public bool? ColorAdded { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public string CountryOfProcessing { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public bool? ElectronicShelfTag { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public DateTime? Exclusive { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public int? NumberOfDigitsSentToScale { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public string ChicagoBaby { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public string TagUom { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public string LinkedItem { get; set; }
        /// <summary>
        /// Extended Property
        /// </summary>
        public string ScaleExtraText { get; set; }
    }
}
