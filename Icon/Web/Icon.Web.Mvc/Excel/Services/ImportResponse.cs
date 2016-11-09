namespace Icon.Web.Mvc.Excel.Services
{
    using System.Collections.Generic;

    public class ImportResponse<T> where T : new()
    {
        public List<T> Items { get; set; }
        public List<T> ErrorItems { get; set; }
        public string ErrorMessage { get; set; }

        public ImportResponse()
        {
            Items = new List<T>();
            ErrorItems = new List<T>();
        }
    }
}