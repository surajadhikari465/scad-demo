namespace WebSupport.DataAccess.Commands
{
    using System.Collections.Generic;

    public class CreateEventsForRegionCommand
    {
        public string Region { get; set; }

        public string EventType { get; set; }

        public IEnumerable<string> ScanCodes { get; set; }
    }
}
