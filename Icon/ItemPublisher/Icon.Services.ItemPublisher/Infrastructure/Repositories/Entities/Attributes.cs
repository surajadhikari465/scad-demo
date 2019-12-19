namespace Icon.Services.ItemPublisher.Repositories.Entities
{
    public class Attributes
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeDisplayName { get; set; }
        public string Description { get; set; }
        public string TraitCode { get; set; }
        public int TraitId { get; set; }
        public string XmlTraitDescription { get; set; }
        public string DataTypeName { get; set; }
        public bool IsPickList { get; set; }

        public bool IsSpecialTransform { get; set; }
    }
}