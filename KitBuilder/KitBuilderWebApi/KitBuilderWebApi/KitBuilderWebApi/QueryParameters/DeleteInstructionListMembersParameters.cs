using System.Collections.Generic;

namespace KitBuilderWebApi.QueryParameters
{
    public class DeleteInstructionListMembersParameters
    {
        public int InstructionListId { get; set; }
        public List<int> InstructionListMemberIds { get; set; }
    }
}