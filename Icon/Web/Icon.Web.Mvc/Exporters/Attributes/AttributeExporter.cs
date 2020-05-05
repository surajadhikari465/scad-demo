using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;
using System.Linq;
using Icon.Web.Mvc.Utility;

namespace Icon.Web.Mvc.Exporters
{
    public class AttributeExporter : BaseAttributeExporter<AttributeViewModel>
    {
        private const int DisplayNameIndex = 0;
        private const int AttributeNameIndex = 1;
        private const int AttributeGroupNameIndex = 2;
        private const int HasUniqueValuesIndex = 3;
        private const int DescriptionIndex = 4;
        private const int DefaultValueIndex = 5;
        private const int IsRequiredIndex = 6;
        private const int SpecialCharactersAllowedIndex = 7;
        private const int TraitCodeIndex = 8;
        private const int DataTypeNameIndex = 9;
        private const int DisplayOrderIndex = 10;
        private const int InitialValueIndex = 11;
        private const int IncrementByIndex = 12;
        private const int InitialMaxIndex = 13;
        private const int DisplayTypeIndex = 14;
        private const int MaxLengthAllowedIndex = 15;
        private const int MinimumNumberIndex = 16;
        private const int MaximumNumberIndex = 17;
        private const int NumberOfDecimalsIndex = 18;
        private const int IsPickListIndex = 19;
        private const int XmlTraitDescriptionIndex = 20;
        private const int PickListDataIndex = 21;
        private const int CharacterSetIndex = 22;
        private const int ItemCountIndex = 23;
        private const int IsActiveIndex = 24;

        AttributeViewModel attributeModel = new AttributeViewModel();

        public AttributeExporter()
            : base()
        {
            AddSpreadsheetColumn(DisplayNameIndex,
               AttributesHelper.AttributesColumnNames.DisplayName,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[DisplayNameIndex].Value = attribute.DisplayName);

            AddSpreadsheetColumn(AttributeNameIndex,
                AttributesHelper.AttributesColumnNames.AttributeName,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[AttributeNameIndex].Value = attribute.AttributeName);

            AddSpreadsheetColumn(AttributeGroupNameIndex,
                AttributesHelper.AttributesColumnNames.AttributeGroupName,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[AttributeGroupNameIndex].Value = AttributesHelper.AttributesColumnNames.GlobalItem);

            AddSpreadsheetColumn(HasUniqueValuesIndex,
                AttributesHelper.AttributesColumnNames.HasUniqueValues,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[HasUniqueValuesIndex].Value = attribute.HasUniqueValues.ToString());

            AddSpreadsheetColumn(DescriptionIndex,
                AttributesHelper.AttributesColumnNames.Description,
                10000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[DescriptionIndex].Value = attribute.Description,
                true);

            AddSpreadsheetColumn(DefaultValueIndex,
                AttributesHelper.AttributesColumnNames.DefaultValue,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[DefaultValueIndex].Value = attribute.DefaultValue);

            AddSpreadsheetColumn(IsRequiredIndex,
                AttributesHelper.AttributesColumnNames.IsRequired,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[IsRequiredIndex].Value = attribute.IsRequired);

            AddSpreadsheetColumn(SpecialCharactersAllowedIndex,
                AttributesHelper.AttributesColumnNames.SpecialCharactersAllowed,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[SpecialCharactersAllowedIndex].Value = attribute.SpecialCharactersAllowed);

            AddSpreadsheetColumn(TraitCodeIndex,
                AttributesHelper.AttributesColumnNames.TraitCode,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[TraitCodeIndex].Value = attribute.TraitCode);

            AddSpreadsheetColumn(DataTypeNameIndex,
                AttributesHelper.AttributesColumnNames.DataTypeName,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[DataTypeNameIndex].Value = attribute.DataTypeName);

            AddSpreadsheetColumn(DisplayOrderIndex,
                AttributesHelper.AttributesColumnNames.DisplayOrder,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[DisplayOrderIndex].Value = attribute.DisplayOrder);

            AddSpreadsheetColumn(InitialValueIndex,
                AttributesHelper.AttributesColumnNames.InitialValue,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[InitialValueIndex].Value = attribute.InitialValue);

            AddSpreadsheetColumn(IncrementByIndex,
                AttributesHelper.AttributesColumnNames.IncrementBy,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[IncrementByIndex].Value = attribute.IncrementBy);

            AddSpreadsheetColumn(InitialMaxIndex,
                AttributesHelper.AttributesColumnNames.InitialMax,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[InitialMaxIndex].Value = attribute.InitialMax);

            AddSpreadsheetColumn(DisplayTypeIndex,
                AttributesHelper.AttributesColumnNames.DisplayType,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[DisplayTypeIndex].Value = attribute.DisplayType);

            AddSpreadsheetColumn(MaxLengthAllowedIndex,
                AttributesHelper.AttributesColumnNames.MaxLengthAllowed,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[MaxLengthAllowedIndex].Value =  attribute.MaxLengthAllowed);

            AddSpreadsheetColumn(MinimumNumberIndex,
                AttributesHelper.AttributesColumnNames.MinimumNumber,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[MinimumNumberIndex].Value = attribute.MinimumNumber);

            AddSpreadsheetColumn(MaximumNumberIndex,
                AttributesHelper.AttributesColumnNames.MaximumNumber,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[MaximumNumberIndex].Value = attribute.MaximumNumber);

            AddSpreadsheetColumn(NumberOfDecimalsIndex,
                AttributesHelper.AttributesColumnNames.NumberOfDecimals,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[NumberOfDecimalsIndex].Value = attribute.NumberOfDecimals);

            AddSpreadsheetColumn(IsPickListIndex,
                AttributesHelper.AttributesColumnNames.IsPickList,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[IsPickListIndex].Value = attribute.IsPickList);

            AddSpreadsheetColumn(XmlTraitDescriptionIndex,
                AttributesHelper.AttributesColumnNames.XmlTraitDescription,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[XmlTraitDescriptionIndex].Value = attribute.XmlTraitDescription);

            AddSpreadsheetColumn(PickListDataIndex,
                AttributesHelper.AttributesColumnNames.PickListData,
                2000,
                HorizontalCellAlignment.Left, 
                (row, attribute) => row.Cells[PickListDataIndex].Value =
                    attribute.PickListData.Any() ? "[" + string.Join(",", attribute.PickListData.Select(a => a.PickListValue)) + "]" : "");

            AddSpreadsheetColumn(CharacterSetIndex,
                AttributesHelper.AttributesColumnNames.CharacterSet,
                2000,
                HorizontalCellAlignment.Left,
                (row, attribute) => row.Cells[CharacterSetIndex].Value =
                    attribute.AvailableCharacterSets.Any()? "[" + string.Join(",", attribute.AvailableCharacterSets.Select(a => a.Name)) + "]" : "");

            AddSpreadsheetColumn(ItemCountIndex,
              AttributesHelper.AttributesColumnNames.ItemCount,
               2000,
               HorizontalCellAlignment.Left,
               (row, attribute) => row.Cells[ItemCountIndex].Value = attribute.ItemCount);

            AddSpreadsheetColumn(IsActiveIndex,
             AttributesHelper.AttributesColumnNames.IsActive,
              2000,
              HorizontalCellAlignment.Left,
              (row, attribute) => row.Cells[IsActiveIndex].Value = attribute.IsActive);
        }


        protected override List<AttributeViewModel> ConvertExportDataToExportAttributeModel()
        {
            List<AttributeViewModel> exportAttributes = ExportData.Select(a => new AttributeViewModel()
            {
                DisplayName = a.DisplayName,
                AttributeName = a.AttributeName,
                AttributeGroupId = a.AttributeGroupId,
                HasUniqueValues = a.HasUniqueValues,
                Description = a.Description,
                DefaultValue = a.DefaultValue,
                IsRequired = a.IsRequired,
                SpecialCharactersAllowed = a.SpecialCharactersAllowed,
                TraitCode = a.TraitCode,
                DataTypeName = a.DataTypeName,
                DisplayOrder = a.DisplayOrder,
                InitialValue = a.InitialValue,
                IncrementBy = a.IncrementBy,
                InitialMax = a.InitialMax,
                DisplayType = a.DisplayType,
                MaxLengthAllowed = a.MaxLengthAllowed,
                MaximumNumber = a.MaximumNumber,
                MinimumNumber = a.MinimumNumber,
                NumberOfDecimals = a.NumberOfDecimals,
                IsPickList = a.IsPickList,
                XmlTraitDescription = a.XmlTraitDescription,
                PickListData = a.PickListData,
                AvailableCharacterSets = a.AvailableCharacterSets,
                ItemCount = a.ItemCount,
                IsActive = a.IsActive
            })
                .ToList();

            return exportAttributes;
        }
    }
}