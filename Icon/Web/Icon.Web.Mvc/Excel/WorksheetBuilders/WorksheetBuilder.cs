using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infragistics.Documents.Excel;
using System.Reflection;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders
{
    public abstract class WorksheetBuilder<T> : IWorksheetBuilder
    {
        protected readonly string worksheetName;
        protected readonly bool displayHeaderRow;

        public abstract IReadOnlyCollection<string> Columns { get; }

        public WorksheetBuilder(string worksheetName, bool displayHeaderRow = true)
        {
            this.worksheetName = worksheetName;
            this.displayHeaderRow = displayHeaderRow;
        }

        public void AppendWorksheet(Workbook workbook)
        {
            var excelModels = GetExcelModels();
            AppendWorksheet(workbook, excelModels);
        }

        public void AppendWorksheet(Workbook workbook, IEnumerable<T> excelModels)
        {
            workbook.Worksheets.Add(worksheetName);
            var worksheet = workbook.Worksheets[worksheetName];

            var columns = typeof(T)
                .GetProperties()
                .Where(p => p.IsDefined(typeof(ExcelColumnAttribute), false))
                .Select(p => new
                {
                    Column = p.GetCustomAttribute<ExcelColumnAttribute>().Column,
                    Width = p.GetCustomAttribute<ExcelColumnAttribute>().Width,
                    Property = p
                })
                .Join(Columns.Select((c, i) => new { Column = c, Index = i }),
                    p => p.Column,
                    c => c.Column,
                    (p, c) => new { p.Property, p.Column, p.Width, c.Index })
                .OrderBy(p => p.Index);

            int rowIndex = 0;
            if (displayHeaderRow)
            {
                for (int j = 0; j < Columns.Count; j++)
                {
                    worksheet.Rows[rowIndex].Cells[j].Value = Columns.ElementAt(j);
                }
                rowIndex++;
            }
            foreach (var model in excelModels)
            {
                foreach (var column in columns)
                {
                    worksheet.Rows[rowIndex].Cells[column.Index].Value = column.Property.GetValue(model);
                }
                rowIndex++;
            }
        }

        protected abstract IEnumerable<T> GetExcelModels();
    }
}