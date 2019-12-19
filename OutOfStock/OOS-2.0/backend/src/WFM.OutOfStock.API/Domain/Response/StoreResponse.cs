namespace WFM.OutOfStock.API.Domain.Response
{
    public sealed class StoreResponse
    {
        public StoreResponse(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}