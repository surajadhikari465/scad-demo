using Icon.Common.DataAccess;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace KitBuilder.DataPurge.Service.Commands
{
	public class PurgeDataCommandHandler : ICommandHandler<PurgeDataCommand>
	{
		private IRepository<PurgeTableInfo> purgeTableInfoRepository;

		public PurgeDataCommandHandler(
			IRepository<PurgeTableInfo> purgeTableInfoRepository)
		{
			this.purgeTableInfoRepository = purgeTableInfoRepository;
		}

		public void Execute(PurgeDataCommand data)
		{
			PurgeData(purgeTableInfoRepository, data.MaxEntries);
		}

		private void PurgeData(IRepository<PurgeTableInfo> repo, int maxEntries)
		{
			var paramBatchSize = new SqlParameter("batchSize", SqlDbType.Int) { Value = maxEntries };
			repo.UnitOfWork.Context.Database.ExecuteSqlCommand("exec dbo.PurgeData @batchSize", paramBatchSize);
		}
	}
}
