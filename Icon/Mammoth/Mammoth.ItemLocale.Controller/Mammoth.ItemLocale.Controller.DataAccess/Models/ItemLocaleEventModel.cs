using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Models
{
    public class ItemLocaleEventModel
    {
        public int QueueId { get; set; }
        public int EventTypeId { get; set; }
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public int StoreNo { get; set; }
        public int? AgeRestriction { get; set; }
        public bool? Authorized { get; set; }
        public bool? CaseDiscount { get; set; }
        public bool Discontinued { get; set; }
        public string LabelTypeDescription { get; set; }
        public bool LocalItem { get; set; }
        public string Locality { get; set; }
        public string ProductCode { get; set; }
        public bool? RestrictedHours { get; set; }
        public string RetailUnit { get; set; }
        public string SignRomanceLong { get; set; }
        public string SignRomanceShort { get; set; }
        public string SignDescription { get; set; }
        public bool? TmDiscount { get; set; }
        public double Msrp { get; set; }
        public bool? OrderedByInfor { get; set; }
        public string AltRetailUOM { get; set; }
        public decimal? AltRetailSize { get; set; }
        public bool DefaultScanCode { get; set; }

        //Vendor Attributes
        public string VendorItemId { get; set; }
        public decimal? VendorCaseSize { get; set; }
        public string VendorKey { get; set; }
        public string VendorCompanyName { get; set; }

        // for one plum
        public bool? ForceTare { get; set; }
        public bool? SendtoCFS { get; set; }
        public string WrappedTareWeight { get; set; }
        public string UnwrappedTareWeight { get; set; }
        public bool? ScaleItem { get; set; }
        public string UseBy { get; set; }
        public int? ShelfLife { get; set; }

        // Extended Attributes (are not required)
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public string ChicagoBaby { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public bool? ColorAdded { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public string CountryOfProcessing { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public bool? ElectronicShelfTag { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public DateTime? Exclusive { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public string LinkedItem { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public int? NumberOfDigitsSentToScale { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public string ScaleExtraText { get; set; }
        /// <summary>
        /// Extended Attributes
        /// </summary>
        public string TagUom { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public string ErrorSource { get; set; }
    }
}
