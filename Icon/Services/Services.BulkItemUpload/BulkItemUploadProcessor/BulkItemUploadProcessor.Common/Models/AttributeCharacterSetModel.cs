namespace BulkItemUploadProcessor.Common.Models
{
    public class AttributeCharacterSetModel
    {
        public int AttributeCharacterSetId { get; set; }
        public int AttributeId { get; set; }
        public CharacterSetModel CharacterSetModel { get; set; }
    }
}