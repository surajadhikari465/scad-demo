using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Commands
{
    /// <summary>
    /// AddItemToItemGroupCommandHandler parameters.
    /// </summary>
    public class AddItemToItemGroupCommand
    {
        /// <summary>
        /// Target Item Group Ia.
        /// </summary>
        public int ItemGroupId { get; set; }

        /// <summary>
        /// ItemId to add.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// ItemGroup type
        /// </summary>
        public ItemGroupTypeId ItemGroupTypeId { get; set; }
    }
}
