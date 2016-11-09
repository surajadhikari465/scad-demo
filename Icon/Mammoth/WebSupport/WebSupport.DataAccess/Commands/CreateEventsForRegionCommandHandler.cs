namespace WebSupport.DataAccess.Commands
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Icon.Common.DataAccess;

    public class CreateEventsForRegionCommandHandler : ICommandHandler<CreateEventsForRegionCommand>
    {
        private IIrmaContextFactory contextFactory;

        public CreateEventsForRegionCommandHandler(IIrmaContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Execute(CreateEventsForRegionCommand data)
        {
            var targetRegionAbbreviation = data.Region;
            var eventType = data.EventType;
            var scanCodes = data.ScanCodes;

            var table = new DataTable();
            table.Columns.Add("Identifier");
            scanCodes.ToList().ForEach(c => table.Rows.Add(c));

            var identifiers = new SqlParameter
            {
                ParameterName = "IdentifiersType",
                Value = table,
                TypeName = "dbo.IdentifiersType"
            };

            var eventTypeName = new SqlParameter("EventTypeName", SqlDbType.VarChar)
            {
                Value = eventType
            };

            var irmaContext = this.contextFactory.CreateContext(targetRegionAbbreviation);
            irmaContext.Database.ExecuteSqlCommand(
                "EXEC mammoth.GenerateEvents @IdentifiersType, @EventTypeName", 
                identifiers, 
                eventTypeName);
        }
    }
}