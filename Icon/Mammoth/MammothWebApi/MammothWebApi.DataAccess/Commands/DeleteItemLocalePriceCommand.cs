namespace MammothWebApi.DataAccess.Commands
{
    public class DeleteItemLocalePriceCommand
    {
        public int BusinessUnitId { get; set; }
        public string ScanCode { get; set; }
        public string Region { get; set; }
    }
}