using Icon.Common.DataAccess;
using KitBuilder.DataPurge.Service.Commands;

namespace KitBuilder.DataPurge.Service.Services
{
	public class DataPurgeService : IDataPurgeService
	{
		private ICommandHandler<PurgeDataCommand> purgeDataCommandHandler;
		public DataPurgeService(ICommandHandler<PurgeDataCommand> purgeDataCommandHandler)
		{
			this.purgeDataCommandHandler = purgeDataCommandHandler;
		}

		public void PurgeData(int maxEntries)
		{
			purgeDataCommandHandler.Execute(new PurgeDataCommand { MaxEntries = maxEntries });
		}
	}
	
}
