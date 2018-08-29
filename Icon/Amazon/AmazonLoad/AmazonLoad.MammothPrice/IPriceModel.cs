using System;

namespace AmazonLoad.MammothPrice
{
    public interface IPriceModel
    {
        int ItemId { get; set; }
        int BusinessUnitId { get; set; }
        string CurrencyCode { get; set; }
        DateTime StartDate { get; set; }
        DateTime? EndDate { get; set; }
        decimal Price { get; set; }
        string ScanCode { get; set; }
        int Multiple { get; set; }
        decimal? PercentOff { get; set; }
        string ItemTypeDesc { get; set; }
        string ItemTypeCode { get; set; }
        string LocaleName { get; set; }

    }
}