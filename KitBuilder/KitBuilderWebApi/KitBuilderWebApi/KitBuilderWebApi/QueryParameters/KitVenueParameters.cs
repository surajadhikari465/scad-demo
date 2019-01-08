namespace KitBuilderWebApi.QueryParameters
{
  public class VenueParameters : BaseParameters
  {
    public int[] ItemIDs { get; set; }

    public VenueParameters()
    {
      PageNumber = 0;
      PageSize = 1000;
    }
  }

  public class KitItemParameters: VenueParameters
  {
    public string[] ScanCodes { get; set; }
  }
}
