namespace MammothWebApi.DataAccess.Models.DataMonster
{
    public class ItemInformation
    {
        public int ItemId { get; set; }
        public int BrandHCID { get; set; }
        public string BrandName { get; set; }
        public string ScanCode { get; set; }
        public string RetailSize { get; set; }
        public string PackageUnit { get; set; }
        public string RetailUOM { get; set; }
        public int SubTeamNumber { get; set; }
        public string SubTeamName { get; set; }
        public string FamilyName { get; set; }
        public int FamilyId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string SubCatName { get; set; }
        public int SubCatId { get; set; }
        public string ClassName { get; set; }
        public int ClassId { get; set; }
        public int TaxClassId { get; set; }
        public string TaxClassDesc { get; set; }
        public string Allergens { get; set; }
        public string Ingredients { get; set; }

    }
}