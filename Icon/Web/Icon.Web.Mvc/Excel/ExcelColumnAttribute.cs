namespace Icon.Web.Mvc.Excel
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnAttribute : Attribute
    {
        public string Column { get; set; }

        public int Width { get; set; }

        public ExcelColumnAttribute(string columnName, int width)
        {
            this.Column = columnName;
            this.Width = width;
        }
    }
}