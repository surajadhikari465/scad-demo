namespace Icon.Web.Mvc.Excel.Services
{
    using Columns;
    using ExcelValidationRuleBuilders.Factories;
    using Icon.Web.Mvc.Excel.Models;
    using System.Collections.Generic;
    using Validators.Factories;
    using WorksheetBuilders.Factories;
    using System;
    using System.Linq;
    using static ExcelHelper;
    public class ItemExcelService : ExcelService<ItemExcelModel>
    {
        public override string MainWorksheetPage { get { return "Items"; } }

        public override IReadOnlyCollection<string> Columns
        {
            get
            {
                return ItemColumns.Columns;
            }
        }

        public override int TemplateNumberOfRows
        {
            get
            {
                return 1000;
            }
        }

        public ItemExcelService(IExcelValidatorFactory<ItemExcelModel> validatorFactory,
            IWorksheetBuilderFactory<ItemExcelModel> worksheetBuilderFactory,
            IExcelValidationRuleBuilderFactory<ItemExcelModel> validationRuleBuilderFactory)
            : base(validatorFactory, worksheetBuilderFactory, validationRuleBuilderFactory)
        {

        }

        public override ExportResponse Export(ExportRequest<ItemExcelModel> request)
        {
            var response = base.Export(request);

            response.ExcelWorkbook.Worksheets[MainWorksheetPage]
                .Columns[ItemColumns.Columns.ToList().IndexOf(ExcelExportColumnNames.ScanCode)]
                .CellFormat.FormatString = "@";

            return response;
        }
    }
}