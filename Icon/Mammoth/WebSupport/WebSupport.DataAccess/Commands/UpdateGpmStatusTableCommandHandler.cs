using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Queries;

namespace WebSupport.DataAccess.Commands
{
    public class UpdateGpmStatusTableCommandHandler : ICommandHandler<UpdateGpmStatusTableCommandParameters>
    {
        private MammothContext mammothDbContext;
        private IQueryHandler<GetGpmStatusParameters, IList<RegionGpmStatus>> getGpmStatusQuery;
        private ICommandHandler<UpsertGpmStatusCommandParameters> upsertGpmStatusCommand;
        private ICommandHandler<DeleteGpmStatusCommandParameters> deleteGpmStatusCommand;

        public UpdateGpmStatusTableCommandHandler(MammothContext connection,
            IQueryHandler<GetGpmStatusParameters, IList<RegionGpmStatus>> getGpmStatusQuery,
            ICommandHandler<UpsertGpmStatusCommandParameters> upsertGpmStatusCommand,
            ICommandHandler<DeleteGpmStatusCommandParameters> deleteGpmStatusCommand)
        {
            this.mammothDbContext = connection;
            this.getGpmStatusQuery = getGpmStatusQuery;
            this.upsertGpmStatusCommand = upsertGpmStatusCommand;
            this.deleteGpmStatusCommand = deleteGpmStatusCommand;
        }

        public void Execute(UpdateGpmStatusTableCommandParameters commandData)
        {
            if (commandData != null & commandData.Regions != null & commandData.Regions.Count > 0)
            {
                var existing = getGpmStatusQuery.Search(new GetGpmStatusParameters());

                var toUpdate = commandData.Regions
                    .Where(p => existing.Select(e => e.Region).Contains(p.Region) &&
                        p.IsGpmEnabled != existing.Single(e => e.Region == p.Region).IsGpmEnabled).ToList();
                var toAdd = commandData.Regions
                    .Where(p => !existing.Select(e => e.Region)
                    .Contains(p.Region)).ToList();
                var toRemove = existing
                    .Where(e => !commandData.Regions.Select(p => p.Region)
                    .Contains(e.Region)).ToList();

                using (var dbContextTransaction = mammothDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var regionToUpdate in toUpdate)
                        {
                            var existingRegion = existing
                                .Single(s => s.Region.Equals(regionToUpdate.Region));
                            if (existingRegion.IsGpmEnabled != regionToUpdate.IsGpmEnabled)
                            {
                                var cmdData = new UpsertGpmStatusCommandParameters(regionToUpdate.Region, regionToUpdate.IsGpmEnabled);
                                upsertGpmStatusCommand.Execute(cmdData);
                            }
                        }

                        foreach (var regionToAdd in toAdd)
                        {
                            var cmdData = new UpsertGpmStatusCommandParameters(regionToAdd.Region, regionToAdd.IsGpmEnabled);
                            upsertGpmStatusCommand.Execute(cmdData);
                        }

                        foreach (var regionToDelete in toRemove)
                        {
                            var cmdData = new DeleteGpmStatusCommandParameters(regionToDelete.Region);
                            deleteGpmStatusCommand.Execute(cmdData);
                        }

                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
