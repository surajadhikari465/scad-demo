namespace Icon.Web.DataAccess.Managers
{
    public class AddLocaleManager : LocaleManager
    {
        public string BusinessUnit { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string StoreAbbreviation { get; set; }
        public string EwicAgencyId { get; set; }
        public string Fax { get; set; }
        public string IrmaStoreId { get; set; }
        public string StorePosType { get; set; }
        public bool Ident { get; set; }
		public string LocalZone  { get; set; }
        public string LiquorLicense  { get; set; }
        public string PrimeMerchantID  { get; set; }
        public string PrimeMerchantIDEncrypted { get; set; }
    }
}