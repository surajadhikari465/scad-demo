using System;
using System.ComponentModel.DataAnnotations;

namespace MammothWebApi.Models
{
    public class ItemLocaleModel
    {
        // 'Core' Attributes
        [MaxLength(2)]
        [Required]
        public string Region { get; set; }
        public int BusinessUnitId { get; set; }

        [MaxLength(13)]
        [Required]
        public string ScanCode { get; set; }
        public bool? CaseDiscount { get; set; }
        public bool? TmDiscount { get; set; }
        public int? AgeRestriction { get; set; }
        public bool? RestrictedHours { get; set; }
        public bool? Authorized { get; set; }
        public bool Discontinued { get; set; }

        [MaxLength(4)]
        public string LabelTypeDescription { get; set; }
        public bool LocalItem { get; set; }

        [MaxLength(15)]
        public string ProductCode { get; set; }

        [MaxLength(25)]
        public string RetailUnit { get; set; }

        [MaxLength(60)]
        public string SignDescription { get; set; }

        [MaxLength(50)]
        public string Locality { get; set; }

        [MaxLength(300)]
        public string SignRomanceLong { get; set; }

        [MaxLength(140)]
        public string SignRomanceShort { get; set; }

        public decimal MSRP { get; set; }
        public bool? OrderedByInfor { get; set; }
        public string AltRetailUOM { get; set; }
        public decimal? AltRetailSize { get; set; }
        public bool? DefaultScanCode { get; set; }
        public int? IrmaItemKey { get; set; }

        //Vendor Attributes
        public string VendorItemId { get; set; }
        public decimal? VendorCaseSize { get; set; }
        public string VendorKey { get; set; }
        public string VendorCompanyName { get; set; }

        // Scale Attributes
        public bool? ForceTare { get; set; }
        public bool? SendtoCFS { get; set; }
        public string WrappedTareWeight { get; set; }
        public string UnwrappedTareWeight { get; set; }
        public bool? ScaleItem { get; set; }
        public string UseBy { get; set; }
        public int? ShelfLife { get; set; }

        // 'Extended' Attributes
        public bool? ColorAdded { get; set; }
        public string CountryOfProcessing { get; set; }
        public string Origin { get; set; }
        public bool? ElectronicShelfTag { get; set; }
        public DateTime? Exclusive { get; set; }
        public int? NumberOfDigitsSentToScale { get; set; }
        public string ChicagoBaby { get; set; }
        public string TagUom { get; set; }
        public string LinkedItem { get; set; }
        public string ScaleExtraText { get; set; }

        public int? PosScaleTare { get; set; }
        public bool? LockedForSale { get; set; }
    }
}