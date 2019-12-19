namespace OutOfStock.Models
{
    public class UserLocation
    {
        public string Region { get; set; }
        public string Store { get; set; }
        public string FriendlyName { get; set; }
        public bool LocationOverride { get; set; }
    }
}