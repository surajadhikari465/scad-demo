namespace Icon.Web.Mvc.Excel.Services
{
    using ExcelValidationRuleBuilders;
    using ExcelValidationRuleBuilders.Factories;
    using Icon.Web.Mvc.Excel.Models;
    using Icon.Web.Mvc.Excel.Validators;
    using Infragistics.Documents.Excel;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Validators.Factories;
    using WorksheetBuilders;
    using WorksheetBuilders.Factories;

    public abstract class ExcelService<T> : IExcelService<T> where T : ExcelModel, new()
    {
        protected readonly IExcelValidatorFactory<T> validatorFactory;
        protected IEnumerable<IExcelValidator<T>> Validators { get; private set; }
        protected IEnumerable<IWorksheetBuilder> WorksheetBuilders { get; private set; }
        protected IEnumerable<IExcelValidationRuleBuilder> ValidationRuleBuilders { get; private set; }
        protected int MaximumRowsToImport { get; set; } = int.MaxValue;

        public abstract string MainWorksheetPage { get; }
        public abstract IReadOnlyCollection<string> Columns { get; }
        public abstract int TemplateNumberOfRows { get; }

        public ExcelService(IExcelValidatorFactory<T> validatorFactory,
            IWorksheetBuilderFactory<T> worksheetBuilderFactory,
            IExcelValidationRuleBuilderFactory<T> validationRuleBuilderFactory)
        {
            this.validatorFactory = validatorFactory;
            this.Validators = this.validatorFactory.CreateValidators();
            this.WorksheetBuilders = worksheetBuilderFactory.CreateWorksheetBuilders();
            this.ValidationRuleBuilders = validationRuleBuilderFactory.CreateBuilders();
        }

        public virtual ImportResponse<T> Import(Workbook book)
        {
            var result = new ImportResponse<T>();

            var headerRow = book.Worksheets
                .First()
                .Rows
                .First()
                .Cells.Select(c => c.Value as string ?? string.Empty);

            var missingHeaders = this.Columns.Except(headerRow);
            if (missingHeaders.Any())
            {
                result.ErrorMessage = string.Format(
                    "The spreadsheet is invalid and missing the following columns: {0}",
                    string.Join(", ", missingHeaders));

                return result;
            }

            var items = ParseModels(book);

            this.Validate(items);

            result.Items = items.AsParallel().Where(i => string.IsNullOrEmpty(i.Error)).ToList();
            result.ErrorItems = items.AsParallel().Except(result.Items.AsParallel()).ToList();

            return result;
        }

        public virtual ExportResponse Export(ExportRequest<T> request)
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Add(MainWorksheetPage);
            var worksheet = workbook.Worksheets[MainWorksheetPage];

            AddRowsToWorksheet(request, worksheet);

            foreach (var worksheetBuilder in WorksheetBuilders)
            {
                worksheetBuilder.AppendWorksheet(workbook);
            }

            int numberOfRows = request.Rows.Count;
            if (request.CreateTemplate)
            {
                numberOfRows = TemplateNumberOfRows;
            }
            foreach (var validationRuleBuilder in ValidationRuleBuilders)
            {
                validationRuleBuilder.AddValidationRule(workbook, numberOfRows);
            }

            return new ExportResponse { ExcelWorkbook = workbook };
        }

        private void AddRowsToWorksheet(ExportRequest<T> request, Worksheet worksheet)
        {
            // Get the properties and columns for the spreadsheet from the ExcelColumn attribute of the
            // model and associate it to the Columns defined for this service in order to get the column indexes.
            var excelColumns = typeof(T)
                .GetProperties()
                .Where(p => p.IsDefined(typeof(ExcelColumnAttribute), false))
                .Select(p => new
                {
                    Property = p,
                    Column = p.GetCustomAttribute<ExcelColumnAttribute>().Column,
                    Width = p.GetCustomAttribute<ExcelColumnAttribute>().Width
                })
                .Join(Columns.Select((c, i) => new { Column = c, Index = i }),
                    p => p.Column,
                    c => c.Column,
                    (p, c) => new { p.Property, p.Column, p.Width, c.Index })
                .OrderBy(p => p.Index);

            foreach (var property in excelColumns)
            {
                worksheet.Columns[property.Index].Width = property.Width;

                var cell = worksheet.Rows[0].Cells[property.Index];
                cell.Value = property.Column;
                cell.CellFormat.Fill = CellFill.CreateSolidFill(Color.LightGreen);
                cell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Black);
                cell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            }

            if (request.Rows.Any())
            {
                for (int i = 0, j = 1; i < request.Rows.Count; i++, j++)
                {
                    foreach (var column in excelColumns)
                    {
                        worksheet.Rows[j].Cells[column.Index].Value = column.Property.GetValue(request.Rows[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Needs to validate the imported spreedsheet and populate errors on the models.
        /// </summary>
        /// <param name="importedModels"></param>
        /// <returns></returns>
        protected virtual void Validate(List<T> importedModels)
        {
            Validators.ToList().ForEach(
                v => v.Validate(importedModels.Where(m => string.IsNullOrWhiteSpace(m.Error))));
        }

        private List<T> ParseModels(Workbook book)
        {
            var allRows = book.Worksheets.First().Rows;
            var headerRow = allRows.First();

            var excelPropertyInfo = headerRow.Cells
                .Select((c, i) => new { Index = i, Column = c.Value.ToString() })
                .Join(
                    typeof(T).GetProperties().Where(p => p.IsDefined(typeof(ExcelColumnAttribute), false)),
                    x => x.Column,
                    pi => pi.GetCustomAttribute<ExcelColumnAttribute>().Column,
                    (x, pi) => new { Index = x.Index, PropertyInfo = pi })
                .ToList();

            var entities = new ConcurrentBag<T>();

            Parallel.ForEach(allRows.Skip(1).Take(MaximumRowsToImport), row =>
            {
                if (row.Cells.Any(c => c.Value != null && !string.IsNullOrWhiteSpace(c.Value.ToString())))
                {
                    var newExcelRowModel = new T();

                    foreach (var pi in excelPropertyInfo)
                    {
                        pi.PropertyInfo.SetValue(
                            newExcelRowModel,
                            row.Cells[pi.Index].Value?.ToString().Trim() ?? string.Empty);
                    }

                    entities.Add(newExcelRowModel);
                }
            });

            return entities.ToList();
        }
    }
}