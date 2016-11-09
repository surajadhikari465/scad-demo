namespace Icon.Web.DataAccess.Commands
{
    public class ApplyMerchTaxAssociationToItemsCommand
    {
        public int MerchandiseHierarchyClassId { get; set; }
        public int TaxHierarchyClassId { get; set; }
    }
}
