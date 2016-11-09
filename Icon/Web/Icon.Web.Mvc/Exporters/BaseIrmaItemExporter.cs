using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public abstract class BaseIrmaItemExporter<T>
    {
        protected const int AsciiA = 65;
        protected const int AsciiZ = 90;
        protected const int firstRow = 1;

        public List<T> ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }

        protected Worksheet itemsWorksheet;
        protected Worksheet brandWorksheet;
        protected Worksheet merchandiseWorksheet;
        protected Worksheet taxWorksheet;
        protected Worksheet browsingWorksheet;
        protected Worksheet merchTaxMapWorksheet;
        protected Worksheet nationalWorksheet;
        protected Worksheet glutenFreeWorksheet;
        protected Worksheet kosherWorksheet;
        protected Worksheet nonGmoWorksheet;
        protected Worksheet organicWorksheet;
        protected Worksheet veganWorksheet;
        protected Dictionary<string, BrandModel> brandHierarchyClassDictionary;
        protected Dictionary<string, string> merchandiseHierarchyClassDictionary;
        protected Dictionary<string, string> taxHierarchyClassesDictionary;
        protected Dictionary<string, string> browsingHierarchyClassDictionary;
        protected Dictionary<string, string> merchandiseTaxHierarchyMappingDictionary;
        protected Dictionary<string, string> nationalHierarchyClassDictionary;
        protected Dictionary<int, string> kosherHierarchyClassDictionary;
        protected Dictionary<int, string> glutenFreeHierarchyClassDictionary;
        protected Dictionary<int, string> nonGmoHierarchyClassDictionary;
        protected Dictionary<int, string> organicHierarchyClassDictionary;
        protected Dictionary<int, string> veganHierarchyClassDictionary;
        protected List<SpreadsheetColumn<ExportItemModel>> spreadsheetColumns;
        protected IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;
        protected IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingsQueryHandler;
        protected IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler;
        private IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery;
        protected string brandColumnPosition;
        protected string merchandiseColumnPosition;
        protected string taxColumnPosition;
        protected string browsingColumnPosition;
        protected string nationalClassColumnPosition;

        public BaseIrmaItemExporter(
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler,
            IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingsQueryHandler,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler,
            IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery
                )
        {
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
            this.getMerchTaxMappingsQueryHandler = getMerchTaxMappingsQueryHandler;
            this.getCertificationAgenciesQueryHandler = getCertificationAgenciesQueryHandler;
            this.getBrandsQuery = getBrandsQuery;

            brandHierarchyClassDictionary = new Dictionary<string, BrandModel>();
            merchandiseHierarchyClassDictionary = new Dictionary<string, string>();
            taxHierarchyClassesDictionary = new Dictionary<string, string>();
            browsingHierarchyClassDictionary = new Dictionary<string, string>();
            nationalHierarchyClassDictionary = new Dictionary<string, string>();
            kosherHierarchyClassDictionary = new Dictionary<int, string>();
            glutenFreeHierarchyClassDictionary = new Dictionary<int, string>();
            nonGmoHierarchyClassDictionary = new Dictionary<int, string>();
            organicHierarchyClassDictionary = new Dictionary<int, string>();
            veganHierarchyClassDictionary = new Dictionary<int, string>();

            spreadsheetColumns = new List<SpreadsheetColumn<ExportItemModel>>();
        }

        public abstract void Export();

        protected abstract void CreateHierarchyExcelValidationRules();
        protected abstract void CreateCustomExcelValidationRules();
        protected abstract List<ExportItemModel> ConvertExportDataToExportItemModel();

        protected void BuildSpreadsheet()
        {
            itemsWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Items");
            brandWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Brands);
            merchandiseWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Merchandise);
            taxWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Tax);
            browsingWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Browsing);
            merchTaxMapWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("MerchTaxMapping");
            nationalWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.National);
            glutenFreeWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Gluten-Free");
            kosherWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Kosher");
            nonGmoWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Non-GMO");
            organicWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Organic");
            veganWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Vegan");

            BuildHierarchyClassDictionaries();
            CreateHierarchyWorksheets();
            CreateHeaderRow();
            AddBrandValidation();
        }

        protected void AddSpreadsheetColumn(int columnIndex, string headerTitle, int width, HorizontalCellAlignment alignment, Action<WorksheetRow, ExportItemModel> setValue)
        {
            spreadsheetColumns.Add(new SpreadsheetColumn<ExportItemModel>
            {
                Index = columnIndex,
                HeaderTitle = headerTitle,
                HeaderBackground = CellFill.CreateSolidFill(Color.LightGreen),
                HeaderForeground = new WorkbookColorInfo(Color.Black),
                IsHeaderFontBold = ExcelDefaultableBoolean.True,
                Width = width,
                Alignment = alignment,
                SetValue = setValue
            });
        }

        protected void CreateHierarchyExcelValidationRule(string hierarchyName, int hierarchyClassCount, int firstRow, int column, int lastRow)
        {
            if (hierarchyClassCount > 0)
            {
                var listRule = new ListDataValidationRule();

                listRule.AllowNull = true;
                listRule.ShowDropdown = true;
                listRule.ErrorMessageDescription = "Invalid value entered.";
                listRule.ErrorMessageTitle = "Validation Error";
                listRule.ErrorStyle = DataValidationErrorStyle.Stop;
                listRule.ShowErrorMessageForInvalidValue = true;

                string valuesFormula = String.Format("='{0}'!$A$1:$A${1}", hierarchyName, hierarchyClassCount);

                listRule.SetValuesFormula(valuesFormula, null);

                var cellRegion = new WorksheetRegion(itemsWorksheet, firstRow, column, lastRow, column);
                var cellCollection = new WorksheetReferenceCollection(cellRegion);

                itemsWorksheet.DataValidationRules.Add(listRule, cellCollection);
            }
        }

        protected void CreateCustomExcelValidationRule(int firstRow, int column, int lastRow, string[] values)
        {
            var listRule = new ListDataValidationRule();

            listRule.AllowNull = true;
            listRule.ShowDropdown = true;
            listRule.ErrorMessageDescription = "Invalid value entered.";
            listRule.ErrorMessageTitle = "Validation Error";
            listRule.ErrorStyle = DataValidationErrorStyle.Stop;
            listRule.ShowErrorMessageForInvalidValue = true;

            listRule.SetValues(values);

            var cellRegion = new WorksheetRegion(itemsWorksheet, firstRow, column, lastRow, column);
            var cellCollection = new WorksheetReferenceCollection(cellRegion);

            itemsWorksheet.DataValidationRules.Add(listRule, cellCollection);
            

           
        }
        void AddBrandValidation()
        {
            var style = itemsWorksheet.Workbook.CreateNewWorksheetCellFormat();
            style.Font.Name = "Verdana";
            style.Font.Height = 240;
            style.Font.ColorInfo = System.Drawing.Color.Red;
            style.Fill = CellFill.CreateSolidFill(Color.LightGreen);

            itemsWorksheet.Workbook.Styles.AddUserDefinedStyle(style, "NewStyle");

            WorksheetCell cell = itemsWorksheet.GetCell("B2");
           // cell.Value = "Test";
            cell.CellFormat.SetFormatting(style);
 

        }

        protected void FormatExcelColumns(int columnIndex)
        {
            itemsWorksheet.Columns[columnIndex].CellFormat.FormatString = "@";
        }

        protected string GetColumnPosition(int columnIndex)
        {
            int columnPositionAscii = AsciiA + columnIndex;
            int columnPositionOverflow = columnPositionAscii - AsciiZ;

            if (columnPositionOverflow > 0)
            {
                string secondCharacter = Convert.ToChar(AsciiA + (columnPositionOverflow - 1)).ToString();
                return "A" + secondCharacter;
            }
            else
            {
                return Convert.ToChar(AsciiA + columnIndex).ToString();
            }
        }

        private void CreateHeaderRow()
        {
            foreach (var column in spreadsheetColumns)
            {
                itemsWorksheet.Rows[0].Cells[column.Index].Value = column.HeaderTitle;
                itemsWorksheet.Rows[0].Cells[column.Index].CellFormat.Fill = column.HeaderBackground;
                itemsWorksheet.Rows[0].Cells[column.Index].CellFormat.Font.ColorInfo = column.HeaderForeground;
                itemsWorksheet.Rows[0].Cells[column.Index].CellFormat.Font.Bold = column.IsHeaderFontBold;

                itemsWorksheet.Columns[column.Index].Width = column.Width;
                itemsWorksheet.Columns[column.Index].CellFormat.Alignment = column.Alignment;
            }
        }

        private void BuildHierarchyClassDictionaries()
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
                        
            merchandiseHierarchyClassDictionary = hierarchyListModel
                .MerchandiseHierarchyList
                .Select(merch => new { Key = merch.HierarchyClassId.ToString(), Value = merch.HierarchyLineage })
                .ToDictionary(m => m.Key, m => m.Value + "|" + m.Key);

            taxHierarchyClassesDictionary = hierarchyListModel
                .TaxHierarchyList
                .Select(tax => new { Key = tax.HierarchyClassId.ToString(), Value = tax.HierarchyLineage })
                .ToDictionary(t => t.Key, t => t.Value + "|" + t.Key);

            browsingHierarchyClassDictionary = hierarchyListModel
                .BrowsingHierarchyList
                .Select(browse => new { Key = browse.HierarchyClassId.ToString(), Value = browse.HierarchyLineage })
                .ToDictionary(br => br.Key, br => br.Value + "|" + br.Key);

            nationalHierarchyClassDictionary = hierarchyListModel
                .NationalHierarchyList
                .Select(nat => new { Key = nat.HierarchyClassId.ToString(), Value = nat.HierarchyLineage })
                .ToDictionary(nat => nat.Key, nat => nat.Value + "|" + nat.Key);

            List<BrandModel> brandList = getBrandsQuery.Search(new GetBrandsParameters());
            brandHierarchyClassDictionary = brandList                
                .Select(brand => new { Key = brand.HierarchyClassId.ToString(), Value = brand })
                .ToDictionary(b => b.Key, b => b.Value);

            List<MerchTaxMappingModel> merchTaxMappingList = getMerchTaxMappingsQueryHandler.Search(new GetMerchTaxMappingsParameters());

            if (merchTaxMappingList != null)
            {
                merchandiseTaxHierarchyMappingDictionary = merchTaxMappingList
                    .Select(mt => new { key = mt.MerchandiseHierarchyClassLineage + '|' + mt.MerchandiseHierarchyClassId, Value = mt.TaxHierarchyClassLineage + '|' + mt.TaxHierarchyClassId })
                    .ToDictionary(m => m.key, m => m.Value);
            }
            else
            {
                merchandiseTaxHierarchyMappingDictionary = new Dictionary<string, string>();
            }

            var certificationAgencyParameters = new GetCertificationAgenciesByTraitParameters();

            certificationAgencyParameters.AgencyTypeTraitCode = TraitCodes.GlutenFree;
            List<HierarchyClass> glutenFreeAgencies = getCertificationAgenciesQueryHandler.Search(certificationAgencyParameters);

            glutenFreeHierarchyClassDictionary = glutenFreeAgencies
                .Select(a => new { Key = a.hierarchyClassID, Value = a.hierarchyClassName })
                .ToDictionary(a => a.Key, a => a.Value + "|" + a.Key);

            certificationAgencyParameters.AgencyTypeTraitCode = TraitCodes.Kosher;
            List<HierarchyClass> kosherAgencies = getCertificationAgenciesQueryHandler.Search(certificationAgencyParameters);

            kosherHierarchyClassDictionary = kosherAgencies
                .Select(a => new { Key = a.hierarchyClassID, Value = a.hierarchyClassName })
                .ToDictionary(a => a.Key, a => a.Value + "|" + a.Key);

            certificationAgencyParameters.AgencyTypeTraitCode = TraitCodes.NonGmo;
            List<HierarchyClass> nonGmoAgencies = getCertificationAgenciesQueryHandler.Search(certificationAgencyParameters);

            nonGmoHierarchyClassDictionary = nonGmoAgencies
                .Select(a => new { Key = a.hierarchyClassID, Value = a.hierarchyClassName })
                .ToDictionary(a => a.Key, a => a.Value + "|" + a.Key);

            certificationAgencyParameters.AgencyTypeTraitCode = TraitCodes.Organic;
            List<HierarchyClass> organicAgencies = getCertificationAgenciesQueryHandler.Search(certificationAgencyParameters);

            organicHierarchyClassDictionary = organicAgencies
                .Select(a => new { Key = a.hierarchyClassID, Value = a.hierarchyClassName })
                .ToDictionary(a => a.Key, a => a.Value + "|" + a.Key);

            certificationAgencyParameters.AgencyTypeTraitCode = TraitCodes.Vegan;
            List<HierarchyClass> veganAgencies = getCertificationAgenciesQueryHandler.Search(certificationAgencyParameters);

            veganHierarchyClassDictionary = veganAgencies
                .Select(a => new { Key = a.hierarchyClassID, Value = a.hierarchyClassName })
                .ToDictionary(a => a.Key, a => a.Value + "|" + a.Key);
        }

        private void CreateHierarchyWorksheets()
        {
            CreateHierarchyWorksheet(brandHierarchyClassDictionary.Select(bh => new {key = bh.Key, value = string.Join("|", bh.Value.HierarchyClassName, bh.Key) }).ToDictionary(dict => dict.key, dict =>dict.value), brandWorksheet);
            CreateHierarchyWorksheet(merchandiseHierarchyClassDictionary, merchandiseWorksheet);
            CreateHierarchyWorksheet(taxHierarchyClassesDictionary, taxWorksheet);
            CreateHierarchyWorksheet(browsingHierarchyClassDictionary, browsingWorksheet);
            CreateHierarchyMappingWorksheet(merchandiseTaxHierarchyMappingDictionary, merchTaxMapWorksheet);
            CreateHierarchyWorksheet(nationalHierarchyClassDictionary, nationalWorksheet);
            CreateHierarchyWorksheet(glutenFreeHierarchyClassDictionary, glutenFreeWorksheet);
            CreateHierarchyWorksheet(kosherHierarchyClassDictionary, kosherWorksheet);
            CreateHierarchyWorksheet(nonGmoHierarchyClassDictionary, nonGmoWorksheet);
            CreateHierarchyWorksheet(organicHierarchyClassDictionary, organicWorksheet);
            CreateHierarchyWorksheet(veganHierarchyClassDictionary, veganWorksheet);
        }

        private void CreateHierarchyWorksheet(Dictionary<string, string> hierarchyClassDictionary, Worksheet hierarchyClassWorksheet)
        {
            int i = 0;
            foreach (string hierarchyClass in hierarchyClassDictionary.Values.OrderBy(hc => hc))
            {
                hierarchyClassWorksheet.Rows[i].Cells[0].Value = hierarchyClass;
                i++;
            }
        }

        private void CreateHierarchyWorksheet(Dictionary<int, string> hierarchyClassDictionary, Worksheet hierarchyClassWorksheet)
        {
            int i = 0;
            foreach (string hierarchyClass in hierarchyClassDictionary.Values.OrderBy(hc => hc))
            {
                hierarchyClassWorksheet.Rows[i].Cells[0].Value = hierarchyClass;
                i++;
            }
        }

        private void CreateHierarchyMappingWorksheet(Dictionary<string, string> hierarchyClassMappingDictionary, Worksheet hierarchyClassWorksheet)
        {
            int i = 0;

            hierarchyClassWorksheet.Rows[i].Cells[0].Value = "Merchandise";
            hierarchyClassWorksheet.Rows[i].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            hierarchyClassWorksheet.Columns[0].Width = 27000;
            hierarchyClassWorksheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Left;

            hierarchyClassWorksheet.Rows[i].Cells[1].Value = "Tax";
            hierarchyClassWorksheet.Rows[i].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            hierarchyClassWorksheet.Columns[1].Width = 15000;
            hierarchyClassWorksheet.Columns[1].CellFormat.Alignment = HorizontalCellAlignment.Left;

            i++;

            foreach (string hierarchyClass in hierarchyClassMappingDictionary.Keys.OrderBy(hc => hc))
            {
                hierarchyClassWorksheet.Rows[i].Cells[0].Value = hierarchyClass;
                hierarchyClassWorksheet.Rows[i].Cells[1].Value = hierarchyClassMappingDictionary[hierarchyClass];
                i++;
            }
        }

    }
}