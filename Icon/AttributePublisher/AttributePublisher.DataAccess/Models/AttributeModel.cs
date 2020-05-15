namespace AttributePublisher.DataAccess.Models
{
    public class AttributeModel
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string TraitCode { get; set; }
        public string XmlTraitDescription { get; set; }
        public string DataType { get; set; }
        public string AttributeGroupName { get; set; }
    }
}
