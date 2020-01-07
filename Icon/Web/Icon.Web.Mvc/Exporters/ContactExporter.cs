using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;
using System.Linq;
using Icon.Web.Mvc.Utility;

namespace Icon.Web.Mvc.Exporters
{
    public class ContactExporter : BaseContactExporter<ContactExportViewModel>
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

        public ContactExporter()
            : base()
        {
            AddSpreadsheetColumn(HierarchyNameIndex,
                ContactHelper.ContactColumnNames.HierarchyName,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[HierarchyNameIndex].Value = contact.HierarchyName);
            

            AddSpreadsheetColumn(HierarchyClassIdIndex,
                ContactHelper.ContactColumnNames.HierarchyClassId,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[HierarchyClassIdIndex].Value = contact.HierarchyClassId);

            AddSpreadsheetColumn(HierarchyClassNameIndex,
                ContactHelper.ContactColumnNames.HierarchyClassName,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[HierarchyClassNameIndex].Value = contact.HierarchyClassName);

            AddSpreadsheetColumn(ContactIdIndex,
                ContactHelper.ContactColumnNames.ContactId,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[ContactIdIndex].Value = contact.ContactId);

            AddSpreadsheetColumn(ContactTypeNameIndex,
                ContactHelper.ContactColumnNames.ContactTypeName,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[ContactTypeNameIndex].Value = contact.ContactTypeName);

            AddSpreadsheetColumn(ContactNameIndex,
                ContactHelper.ContactColumnNames.ContactName,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[ContactNameIndex].Value = contact.ContactName);

            AddSpreadsheetColumn(EmailIndex,
                ContactHelper.ContactColumnNames.Email,
                10000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[EmailIndex].Value = contact.Email);

            AddSpreadsheetColumn(TitleIndex,
                ContactHelper.ContactColumnNames.Title,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[TitleIndex].Value = contact.Title);

            AddSpreadsheetColumn(AddressLine1Index,
                ContactHelper.ContactColumnNames.AddressLine1,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[AddressLine1Index].Value = contact.AddressLine1);

            AddSpreadsheetColumn(AddressLine2Index,
                ContactHelper.ContactColumnNames.AddressLine2,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[AddressLine2Index].Value = contact.AddressLine2);

            AddSpreadsheetColumn(CityIndex,
                ContactHelper.ContactColumnNames.City,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[CityIndex].Value = contact.City);

            AddSpreadsheetColumn(StateIndex,
                ContactHelper.ContactColumnNames.State,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[StateIndex].Value = contact.State);

            AddSpreadsheetColumn(ZipCodeIndex,
                ContactHelper.ContactColumnNames.ZipCode,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[ZipCodeIndex].Value = contact.ZipCode);

            AddSpreadsheetColumn(CountryIndex,
                ContactHelper.ContactColumnNames.Country,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[CountryIndex].Value = contact.Country);

            AddSpreadsheetColumn(PhoneNumber1Index,
                ContactHelper.ContactColumnNames.PhoneNumber1,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[PhoneNumber1Index].Value = contact.PhoneNumber1);

            AddSpreadsheetColumn(PhoneNumber2Index,
                ContactHelper.ContactColumnNames.PhoneNumber2,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[PhoneNumber2Index].Value = contact.PhoneNumber2);

            AddSpreadsheetColumn(WebsiteURLIndex,
                ContactHelper.ContactColumnNames.WebsiteURL,
                2000,
                HorizontalCellAlignment.Left,
                (row, contact) => row.Cells[WebsiteURLIndex].Value = contact.WebsiteURL);
           
        }

        protected override List<ContactExportViewModel> ConvertExportDataToExportContactModel()
        {
            List<ContactExportViewModel> exportContact = ExportData.Select(c => new ContactExportViewModel()
            {
                HierarchyName = c.HierarchyName,
                HierarchyClassId = c.HierarchyClassId,
                HierarchyClassName = c.HierarchyClassName,
                ContactId = c.ContactId,
                ContactTypeName = c.ContactTypeName,
                ContactName = c.ContactName,
                Email = c.Email,
                Title = c.Title,
                AddressLine1 = c.AddressLine1,
                AddressLine2 = c.AddressLine2,
                City = c.City,
                State = c.State,
                ZipCode = c.ZipCode,
                Country = c.Country,
                PhoneNumber1 = c.PhoneNumber1,
                PhoneNumber2 = c.PhoneNumber2,
                WebsiteURL = c.WebsiteURL
            }).OrderBy(c => c.HierarchyName).ThenBy(c => c.HierarchyClassName).ToList();

            return exportContact;
        }
    }
}