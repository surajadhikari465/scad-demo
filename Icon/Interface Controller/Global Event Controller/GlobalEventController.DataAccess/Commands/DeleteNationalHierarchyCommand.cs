using System;

namespace GlobalEventController.DataAccess.Commands
{
   public class DeleteNationalHierarchyCommand
    {
        public int IconId { get; set; }
        public int? Level { get; set; }
        public int? IrmaId { get; set; }
    }
}
