namespace GPMService.Producer.Model.DBModel
{
    internal class MessageSequenceModel
    {
        public int MessageSequenceID { get; set; }
        public string PatchFamilyID { get; set; }
        public string LastProcessedGpmSequenceID { get; set; }
    }
}
