using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Common
{
    public class IconItemLastChangeModel
    {
        public int ItemId { get; set; }
        public string ValidationDate { get; set; }
        public string ScanCode { get; set; }
        public string ScanCodeType { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public string FoodStampEligible { get; set; }
        public string Tare { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string TaxClassName { get; set; }
        public string NationalClassCode { get; set; }
        public bool AreNutriFactsUpdated { get; set; }
        public string SubTeamName { get; set; }
        public int SubTeamNo { get; set; }
        public int DeptNo { get; set; }
        public bool SubTeamNotAligned { get; set; }
        public string RetailUom { get; set; }
        public Decimal RetailSize { get; set; }
  

        public IconItemLastChangeModel() { }

        public IconItemLastChangeModel(ValidatedItemModel validatedItemModel)
        {
            ItemId = validatedItemModel.ItemId;
            ValidationDate = validatedItemModel.ValidationDate;
            ScanCode = validatedItemModel.ScanCode;
            ScanCodeType = validatedItemModel.ScanCodeType;
            ProductDescription = validatedItemModel.ProductDescription;
            PosDescription = validatedItemModel.PosDescription;
            PackageUnit = validatedItemModel.PackageUnit;
            FoodStampEligible = validatedItemModel.FoodStampEligible;
            Tare = validatedItemModel.Tare;
            BrandId = validatedItemModel.BrandId;
            BrandName = validatedItemModel.BrandName;
            TaxClassName = validatedItemModel.TaxClassName;
            SubTeamName = validatedItemModel.SubTeamName;
            SubTeamNo = validatedItemModel.SubTeamNo;
            DeptNo = validatedItemModel.DeptNo;
            SubTeamNotAligned = validatedItemModel.SubTeamNotAligned;
            NationalClassCode = validatedItemModel.NationalClassCode;
            RetailUom = validatedItemModel.RetailUom;
            RetailSize = validatedItemModel.RetailSize;
        }
    }
}
