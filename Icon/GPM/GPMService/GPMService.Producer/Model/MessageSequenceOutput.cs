namespace GPMService.Producer.Model
{
    internal class MessageSequenceOutput
    {
        public string PatchFamilyId { get; set; }
        public int SequenceID { get; set; }
        public int? LastProcessedGpmSequenceID { get; set; }
        public bool IsInSequence { get; set; }
        public bool IsAlreadyProcessed { get; set; }
    }
}
