using System.Collections.Generic;

namespace AttributePublisher.DataAccess.Models
{
    public class AttributeModel
    {
        public int AttributeId { get; set; }
        public string Description { get; set; }
        public string TraitCode { get; set; }
        public int? MaxLengthAllowed { get; set; }
        public string MinimumNumber { get; set; }
        public string MaximumNumber { get; set; }
        public string NumberOfDecimals { get; set; }
        public string XmlTraitDescription { get; set; }
        public string CharacterSetRegexPattern { get; set; }
        public int AttributeGroupId { get; set; }
        public string AttributeGroupName { get; set; }
        public string DataType { get; set; }
        public bool IsPickList { get; set; }
        public List<string> PickListValues { get; set; }
    }
}