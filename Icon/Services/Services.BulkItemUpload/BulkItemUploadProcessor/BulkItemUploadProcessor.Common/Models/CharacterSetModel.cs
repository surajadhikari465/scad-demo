namespace BulkItemUploadProcessor.Common.Models
{
    public class CharacterSetModel
    {
        public int CharacterSetId { get; set; }
        public string Name { get; set; }
        public string RegEx { get; set; }
        public bool IsSelected { get; set; }
    }
}