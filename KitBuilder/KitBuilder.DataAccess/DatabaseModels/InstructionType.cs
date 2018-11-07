using System.Collections.Generic;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class InstructionType
    {
        public InstructionType()
        {
            InstructionList = new HashSet<InstructionList>();
        }

        public int InstructionTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<InstructionList> InstructionList { get; set; }
    }
}
