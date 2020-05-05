using Icon.Web.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Icon.Common.Models;
using Icon.Common;
using Icon.Web.Mvc.Utility;

namespace Icon.Web.Mvc.Models
{
    public class AttributeViewModel
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }
        [Display(Name = "Data Type Name")]
        public string DataTypeName { get; set; }
        [Display(Name = "Trait Code")]
        public string TraitCode { get; set; }

        public string Description { get; set; }

        [DisplayAttribute(Name = "Data Type")]
        public int DataTypeId { get; set; }

        [Display(Name = "Max Length of Attribute Value")]
        public int? MaxLengthAllowed { get; set; }

        public string SpecialCharactersAllowed { get; set; }
        public bool IsSpecialCharactersSelected { get; set; }
        public string SpecialCharacterSetSelected { get; set; }
        public bool IsPickList { get; set; }
        public int GridColumnWidth { get; set; }
        public bool IsReadOnly { get; set; }
        [DisplayAttribute(Name = "Required")]
        public bool IsRequired { get; set; }
        public List<PickListModel> PickListData { get; set; }
        [Display(Name = "Minimum Number")]
        public string MinimumNumber { get; set; }
        [Display(Name = "Maximum Number")]
        public string MaximumNumber { get; set; }
        [Display(Name = "Number of Decimals")]
        public string NumberOfDecimals { get; set; }
        public string CharacterSetRegexPattern { get; set; }
        public Enums.WriteAccess UserWriteAccess { get; set; }
        public IEnumerable<SelectListItem> AvailableDataTypes { get; set; }
        public IEnumerable<SelectListItem> AvailableDefaultValuesForBoolean { get; set; }
        public List<CharacterSetModel> AvailableCharacterSets{ get; set; }
        public ActionEnum Action { get; set; }
        public int? AttributeGroupId { get; set; }
        public bool? HasUniqueValues { get; set; }
        [Display(Name = "Default Value (Optional)")]
        public string DefaultValue { get; set; }
        public int? InitialValue { get; set; }
        public int? IncrementBy { get; set; }
        public int? InitialMax { get; set; }
        public string DisplayType { get; set; }
        public string XmlTraitDescription { get; set; }

        public string AttributeGroupName { get; set; }
        public int? ItemCount { get; set; }
        [Display(Name = AttributesHelper.AttributesColumnNames.IsActive)]
        public bool IsActive { get; set; }
        
        public AttributeViewModel()
        {
        }

        public AttributeViewModel(string specialCharactersAllowed)
        {
            this.SpecialCharactersAllowed = specialCharactersAllowed;
        }

        public bool IsSystemAttribute
        {
            get
            {
                if (this.AttributeName == Constants.Attributes.CreatedBy ||
                    this.AttributeName == Constants.Attributes.CreatedDateTimeUtc ||
                    this.AttributeName == Constants.Attributes.ModifiedBy ||
                    this.AttributeName == Constants.Attributes.ModifiedDateTimeUtc)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
          
        }
        public AttributeViewModel(AttributeModel model)
        {
            this.AttributeId = model.AttributeId;
            this.AttributeName = model.AttributeName;
            this.Description = model.Description;
            this.DisplayName = model.DisplayName;
            this.DisplayOrder = model.DisplayOrder;
            this.DataTypeName = model.DataTypeName;
            this.IsPickList = model.IsPickList;
            this.GridColumnWidth = model.GridColumnWidth;
            this.IsReadOnly = model.IsReadOnly;
            this.IsRequired = model.IsRequired;
            this.PickListData = model?.PickListData == null ? new List<PickListModel>() : model.PickListData.ToList();
            this.MinimumNumber = model.MinimumNumber;
            this.MaximumNumber = model.MaximumNumber;
            this.NumberOfDecimals = model.NumberOfDecimals;
            this.CharacterSetRegexPattern = model.CharacterSetRegexPattern;
            this.MaxLengthAllowed = model.MaxLengthAllowed;
            this.HasUniqueValues = model.HasUniqueValues;
            this.AttributeGroupId = model.AttributeGroupId;
            this.DefaultValue = model.DefaultValue;
            this.SpecialCharactersAllowed = model.SpecialCharactersAllowed;
            this.InitialValue = model.InitialValue;
            this.IncrementBy = model.IncrementBy;
            this.InitialMax = model.InitialMax;
            this.TraitCode = model.TraitCode;
            this.DisplayType = model.DisplayType;
            this.AvailableCharacterSets = new List<CharacterSetModel>();
            this.XmlTraitDescription = model.XmlTraitDescription;            
            this.ItemCount = model.ItemCount;
            this.IsActive = model.IsActive;
        }
    }
}