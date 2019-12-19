using Icon.Web.Mvc.Excel.Columns;
using Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders.Factories;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Validators.Factories;
using Icon.Web.Mvc.Excel.WorksheetBuilders.Factories;
using System.Collections.Generic;
using System.Linq;
using Infragistics.Documents.Excel;
using static Icon.Web.Mvc.Excel.ExcelHelper;

namespace Icon.Web.Mvc.Excel.Services
{
    public class NewItemExcelService : ExcelService<NewItemExcelModel>
    {
        private bool useConsolidatedItemColumns;

        public override IReadOnlyCollection<string> Columns
        {
            get
            {
                if (useConsolidatedItemColumns)
                    return ItemColumns.Columns;
                else
                    return NewItemColumns.Columns;
            }
        }

        public override string MainWorksheetPage
        {
            get
            {
                return "Items";
            }
        }

        public override int TemplateNumberOfRows
        {
            get
            {
                return 1000;
            }
        }

        public NewItemExcelService(IExcelValidatorFactory<NewItemExcelModel> validatorFactory,
            IWorksheetBuilderFactory<NewItemExcelModel> worksheetBuilderFactory,
            IExcelValidationRuleBuilderFactory<NewItemExcelModel> validationRuleBuilderFactory) 
            : base(validatorFactory, worksheetBuilderFactory, validationRuleBuilderFactory) { }

        public override ImportResponse<NewItemExcelModel> Import(Workbook book)
        {
            CalculateColumnsForImport(book);
            return base.Import(book);
        }

        private void CalculateColumnsForImport(Workbook book)
        {
            var workbookColumns = book.Worksheets[0].Rows[0].Cells.Select(c => c.Value?.ToString()).ToList();

            if (workbookColumns.Last() == ExcelExportColumnNames.Error)
                workbookColumns.Remove(workbookColumns.Last());

            if (ItemColumns.Columns.SequenceEqual(workbookColumns))
                useConsolidatedItemColumns = true;
        }
    }
}