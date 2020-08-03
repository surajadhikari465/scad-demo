namespace Icon.Web.DataAccess.Commands
{
    /// <summary>
    /// Set Primary Item for Item Group Command Parameters.
    /// </summary>
    public class SetPrimaryItemGroupItemCommand
    {
        /// <summary>
        /// ItemGroupId for the item group to update.
        /// </summary>
        public int ItemGroupId { get; set; }

        /// <summary>
        /// New primary Item Id.
        /// </summary>
        public int PrimaryItemId { get; set; }
    }
}
