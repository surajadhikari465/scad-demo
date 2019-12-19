namespace IrmaMobile.Domain.Models
{
    public class SubteamModel
    {
        public int SubteamNo { get; set; }
        public string SubteamName { get; set; }
        public int SubteamTypeId { get; set; }
        public bool IsFixedSpoilage { get; set; }
        public bool IsUnrestricted { get; set; }
    }
}
