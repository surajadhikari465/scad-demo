using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Exporters
{
    public class ContactNewTemplateExporter : BaseNewContactExporter<ContactExportViewModel>
    {
       
        private const int HierarchyNameIndex = 0;
        private const int HierarchyClassIdIndex = 1;
        private const int HierarchyClassNameIndex = 2;
        private const int ContactIdIndex = 3;
        private const int ContactTypeNameIndex = 4;
        private const int ContactNameIndex = 5;
        private const int EmailIndex = 6;
        private const int TitleIndex = 7;
        private const int AddressLine1Index = 8;
        private const int AddressLine2Index = 9;
        private const int CityIndex = 10;
        private const int StateIndex = 11;
        private const int ZipCodeIndex = 12;
        private const int CountryIndex = 13;
        private const int PhoneNumber1Index = 14;
        private const int PhoneNumber2Index = 15;
        private const int WebsiteURLIndex = 16;

        ContactExportViewModel contactModel = new ContactExportViewModel();
        public ContactNewTemplateExporter(
            IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler,
            IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery,
            IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypeQuery)
            : base(getHierarchyClassesQueryHandler, getContactsQuery, getContactTypeQuery)
        {
        }

        public override void Export(List<Dictionary<string, object>> results = null)
        {
            base.BuildSpreadsheet();
           
            CreateExcelValidationRules();
            
            if (results != null)
            {
                base.AddRows(results);
            }
        }

        protected override List<ContactExportViewModel> ConvertExportDataToExportContactModel()
        {
            // A template won't contain any data to convert.
            return new List<ContactExportViewModel>();
        }

        protected override void CreateExcelValidationRules()
        {
            base.CreateContactRuleExcelValidationRule(ContactHelper.ContactColumnNames.HierarchyName, ContactHelper.ContactColumnNames.HierarchyName, base.hierarchyNameDictionary.Values.Count);
            base.CreateContactRuleExcelValidationRule(ContactHelper.ContactColumnNames.ContactTypeName, ContactHelper.ContactColumnNames.ContactTypeName, base.contactTypeDictionary.Values.Count);
        }

        public override void AddSpreadsheetColumns()
        {
            int currentIndex = 0;

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.HierarchyName,
                ContactHelper.ContactColumnNames.HierarchyName,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[HierarchyNameIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.HierarchyClassName,
                ContactHelper.ContactColumnNames.HierarchyClassName,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[HierarchyClassNameIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.ContactId,
                ContactHelper.ContactColumnNames.ContactId,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[ContactIdIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.ContactTypeName,
                ContactHelper.ContactColumnNames.ContactTypeName,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[ContactTypeNameIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.ContactName,
                ContactHelper.ContactColumnNames.ContactName,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[ContactNameIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.Email,
                ContactHelper.ContactColumnNames.Email,
                10000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[EmailIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.Title,
                ContactHelper.ContactColumnNames.Title,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[TitleIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.AddressLine1,
                ContactHelper.ContactColumnNames.AddressLine1,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[AddressLine1Index].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.AddressLine2,
                ContactHelper.ContactColumnNames.AddressLine2,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[AddressLine2Index].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.City,
                ContactHelper.ContactColumnNames.City,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[CityIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.State,
                ContactHelper.ContactColumnNames.State,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[StateIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.ZipCode,
                ContactHelper.ContactColumnNames.ZipCode,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[ZipCodeIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.Country,
                ContactHelper.ContactColumnNames.Country,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[CountryIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.PhoneNumber1,
                ContactHelper.ContactColumnNames.PhoneNumber1,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[PhoneNumber1Index].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.PhoneNumber2,
                ContactHelper.ContactColumnNames.PhoneNumber2,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[PhoneNumber2Index].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(ContactHelper.ContactColumnNames.WebsiteURL,
                ContactHelper.ContactColumnNames.WebsiteURL,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[WebsiteURLIndex].Value = String.Empty,
                ref currentIndex);

        }
    }
}