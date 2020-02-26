using System.Collections.Generic;

namespace Icon.Common.Models
{
    public class AttributeModel
    {
        public int AttributeId { get; set; }
        public string DisplayName { get; set; }
        public string AttributeName { get; set; }
        public int? AttributeGroupId { get; set; }
        public bool? HasUniqueValues { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public bool IsRequired { get; set; }
        public string SpecialCharactersAllowed { get; set; }
        public string TraitCode { get; set; }
        public int? DataTypeId { get; set; }
        public string DataTypeName { get; set; }
        public int? DisplayOrder { get; set; }
        public int? InitialValue { get; set; }
        public int? IncrementBy { get; set; }
        public int? InitialMax { get; set; }
        public string DisplayType { get; set; }
        public int? MaxLengthAllowed { get; set; }
        public string MinimumNumber { get; set; }
        public string MaximumNumber { get; set; }
        public string NumberOfDecimals { get; set; }
        public bool IsPickList { get; set; }
        public int GridColumnWidth { get; set; }
        public bool IsReadOnly { get; set; }
        public IEnumerable<PickListModel> PickListData { get; set; }
        public IEnumerable<AttributeCharacterSetModel> CharacterSets { get; set; }
        public string CharacterSetRegexPattern { get; set; }
        public string XmlTraitDescription { get; set; }
        public int? ItemCount { get; set; }
    }
}
