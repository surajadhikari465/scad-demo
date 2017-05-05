using System;

namespace GlobalEventController.DataAccess.Commands
{
   public class DeleteNationalHierarchyCommand
    {
        public int iconId { get; set; }
        public int? level { get; set; }
        public int? irmaId { get; set; }
    }
}
