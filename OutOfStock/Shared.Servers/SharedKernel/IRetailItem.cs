namespace SharedKernel
{
    public interface IRetailItem : IProduct
    {
        string VendorKey { get; }
        string VendorItemNumber { get; }
        string TeamName { get; }
        string SubTeamName { get; }
        string PriceType { get; }
        string PepoleSoftBusinessUnit { get; }
        double Cost { get; }
        double Price { get; }
        decimal CaseSize { get; }
    }
}
