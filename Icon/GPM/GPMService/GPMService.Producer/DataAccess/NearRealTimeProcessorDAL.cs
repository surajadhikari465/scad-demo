using GPMService.Producer.Model.DBModel;
using Icon.DbContextFactory;
using Mammoth.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GPMService.Producer.DataAccess
{
    internal class NearRealTimeProcessorDAL : INearRealTimeProcessorDAL
    {
        IDbContextFactory<MammothContext> mammothContextFactory;

        public NearRealTimeProcessorDAL(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }  

        public IList<MessageSequenceModel> GetLastSequence(string correlationID)
        {
            IList<MessageSequenceModel> messageSequenceData = new List<MessageSequenceModel>();
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = 10;
                string getLastSequenceQuery = $@"SELECT 
                    MessageSequenceID, 
                    PatchFamilyID, 
                    PatchFamilySequenceID AS LastProcessedGpmSequenceID 
                    FROM gpm.MessageSequence 
                    WHERE PatchFamilyID = @CorrelationID";
                messageSequenceData = mammothContext
                    .Database
                    .SqlQuery<MessageSequenceModel>(
                    getLastSequenceQuery,
                    new SqlParameter("@CorrelationID", correlationID)
                    ).ToList();
            }
            return messageSequenceData;
        }
    }
}
