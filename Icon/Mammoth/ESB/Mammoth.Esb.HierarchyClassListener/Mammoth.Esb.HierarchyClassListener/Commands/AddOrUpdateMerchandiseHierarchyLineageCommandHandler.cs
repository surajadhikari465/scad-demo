using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Linq;
using Icon.Common;
using System.Data;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class AddOrUpdateMerchandiseHierarchyLineageCommandHandler : ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand>
    {
        private IDbProvider dbProvider;
        const string udtType = "dbo.HierarchyMerchandiseClassType";

        public AddOrUpdateMerchandiseHierarchyLineageCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            string levelName = data.HierarchyClasses[0].HierarchyLevelName;

            switch (levelName)
            {
                case Constants.Merchandise.HierarchyLevels.Segment:
                    UpdateSegments(data);
                    break;
                case Constants.Merchandise.HierarchyLevels.Family:
                    UpdateFamilies(data);
                    break;
                case Constants.Merchandise.HierarchyLevels.Class:
                    UpdateClasses(data);
                    break;
                case Constants.Merchandise.HierarchyLevels.Gs1Brick:
                    UpdateGs1Bricks(data);
                    break;
                case Constants.Merchandise.HierarchyLevels.SubBrick:
                    UpdateSubBricks(data);
                    break;
            }
        }

        private void UpdateSegments(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                sql: "dbo.AddOrUpdateHierarchyMerchandiseSegment",
                param: new
                {
                    hierarchy = data.HierarchyClasses.Select(x => new
                    {
                        SegmentHCID = x.HierarchyClassId,
                        FamilyHCID = (int?)null,
                        ClassHCID = (int?)null,
                        BrickHCID = (int?)null,
                        SubBrickHCID =(int?)null
                    })
                    .ToDataTable()
                    .AsTableValuedParameter(udtType)
                },
            transaction: dbProvider.Transaction,
            commandType: CommandType.StoredProcedure);
        }

        private void UpdateFamilies(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                sql: "dbo.AddOrUpdateHierarchyMerchandiseFamily",
                param: new
                {
                    hierarchy = data.HierarchyClasses.Select(x => new
                    {
                        SegmentHCID = x.HierarchyClassParentId,
                        FamilyHCID = x.HierarchyClassId,
                        ClassHCID = (int?)null,
                        BrickHCID = (int?)null,
                        SubBrickHCID =(int?)null
                    })
                    .ToDataTable()
                    .AsTableValuedParameter(udtType)
                },
            transaction: dbProvider.Transaction,
            commandType: CommandType.StoredProcedure);
        }

        private void UpdateClasses(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
             dbProvider.Connection.Execute(
                sql: "dbo.AddOrUpdateHierarchyMerchandiseClass",
                param: new
                {
                    hierarchy = data.HierarchyClasses.Select(x => new
                    {
                        SegmentHCID = (int?)null,
                        FamilyHCID = x.HierarchyClassParentId,
                        ClassHCID = x.HierarchyClassId,
                        BrickHCID = (int?)null,
                        SubBrickHCID =(int?)null
                    })
                    .ToDataTable()
                    .AsTableValuedParameter(udtType)
                },
            transaction: dbProvider.Transaction,
            commandType: CommandType.StoredProcedure);
        }

        private void UpdateGs1Bricks(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                sql: "dbo.AddOrUpdateHierarchyMerchandiseBrick",
                param: new
                {
                    hierarchy = data.HierarchyClasses.Select(x => new
                    {
                        SegmentHCID = (int?)null,
                        FamilyHCID = (int?)null,
                        ClassHCID = x.HierarchyClassParentId,
                        BrickHCID = x.HierarchyClassId,
                        SubBrickHCID =(int?)null
                    })
                    .ToDataTable()
                    .AsTableValuedParameter(udtType)
                },
            transaction: dbProvider.Transaction,
            commandType: CommandType.StoredProcedure);
        }

        private void UpdateSubBricks(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                sql: "dbo.AddOrUpdateHierarchyMerchandiseSubBrick",
                param: new
                {
                    hierarchy = data.HierarchyClasses.Select(x => new
                    {
                        SegmentHCID = (int?)null,
                        FamilyHCID = (int?)null,
                        ClassHCID = (int?)null,
                        BrickHCID = x.HierarchyClassParentId,
                        SubBrickHCID =x.HierarchyClassId
                    })
                    .ToDataTable()
                    .AsTableValuedParameter(udtType)
                },
            transaction: dbProvider.Transaction,
            commandType: CommandType.StoredProcedure);
        }
    }
}