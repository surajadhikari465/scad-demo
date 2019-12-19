using System;
using System.Globalization;

namespace Icon.Web.Mvc.Models.ItemHistory
{
    public abstract class AbstractRevision
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string User { get; set; }
        public DateTime Date { get; set; }

        public string GetDisplayDate()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(this.Date, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"))
                .ToString("g", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}