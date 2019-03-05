namespace KitBuilderWebApi.Models
{
    public class LinkGroupWithRegions
    {
        public int LinkGroupId { get; set; }
        public string[] Regions { get; set; }

        public string FormattedRegions()
        {
            return string.Join(",", this.Regions);
        }
    }
}