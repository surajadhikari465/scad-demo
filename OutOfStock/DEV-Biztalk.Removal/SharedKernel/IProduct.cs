namespace SharedKernel
{
    public interface IProduct
    {
        UPC UPC { get; }
        string Brand { get; }
        string BrandName { get; }
        string LongDescription { get; }
        string Size { get; }
        string UOM { get; }
        string CategoryName { get; }
        string ClassName { get; }
    }
}
