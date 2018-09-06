namespace KitBuilderWebApi.DataAccess.Dto
{
    public class InstructionListMemberDto
    {
        public int InstructionListId { get; set; }
        public int InstructionListMemberId { get; set; }
        public int Sequence { get; set; }
        public string Group { get; set; }
        public string Member { get; set; }
    }
}