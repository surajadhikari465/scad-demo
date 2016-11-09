using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class AddOrUpdateMerchandiseHierarchyLineageCommandHandler : ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand>
    {
        private IDbProvider dbProvider;

        public AddOrUpdateMerchandiseHierarchyLineageCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            string levelName = data.HierarchyClasses[0].HierarchyLevelName;
            List<MerchandiseHierarchyModel> models = new List<MerchandiseHierarchyModel>();

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
                @"CREATE TABLE #SegmentIds (SegmentHCID int)", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"INSERT INTO #SegmentIds VALUES (@SegmentHcid)",
                data.HierarchyClasses.Select(hc => new MerchandiseHierarchyModel { SegmentHcid = hc.HierarchyClassId }),
                dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID)
                  SELECT SegmentHCID 
                  FROM #SegmentIds
                  WHERE SegmentHCID NOT IN
                  (
                      SELECT SegmentHCID FROM dbo.Hierarchy_Merchandise
                  )",
                null,
                dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"DROP TABLE #SegmentIds", null, dbProvider.Transaction);
        }

        private void UpdateFamilies(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                @"CREATE TABLE #TempMerchandiseHierarchy
                  (
                      SegmentHCID int null,
                      FamilyHCID int null,
                      ClassHCID int null,
                      BrickHCID int null,
                      SubBrickHCID int null
                  )", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                  @"INSERT INTO #TempMerchandiseHierarchy 
                  VALUES (@SegmentHcid, @FamilyHcid, @ClassHcid, @BrickHcid, @SubBrickHcid)",
                    data.HierarchyClasses.Select(hc => new MerchandiseHierarchyModel
                    {
                        SegmentHcid = hc.HierarchyClassParentId,
                        FamilyHcid = hc.HierarchyClassId
                    }), dbProvider.Transaction);
            dbProvider.Connection.Execute(
                  @"UPDATE hm
                      SET FamilyHCID = temp.FamilyHCID
                  FROM dbo.Hierarchy_Merchandise hm
                  JOIN #TempMerchandiseHierarchy temp ON hm.SegmentHCID = temp.SegmentHCID
                      AND hm.FamilyHCID IS NULL", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                  @"INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID)
                  SELECT *
                  FROM #TempMerchandiseHierarchy temp
                  WHERE temp.FamilyHCID NOT IN
                  (
                      SELECT FamilyHCID FROM dbo.Hierarchy_Merchandise
                  )", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"DROP TABLE #TempMerchandiseHierarchy", null, dbProvider.Transaction);
        }

        private void UpdateClasses(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                @"CREATE TABLE #TempMerchandiseHierarchy
                  (
                      SegmentHCID int null,
                      FamilyHCID int null,
                      ClassHCID int null,
                      BrickHCID int null,
                      SubBrickHCID int null
                  )", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"INSERT INTO #TempMerchandiseHierarchy 
                  VALUES (@SegmentHcid, @FamilyHcid, @ClassHcid, @BrickHcid, @SubBrickHcid)",
                data.HierarchyClasses.Select(hc => new MerchandiseHierarchyModel
                {
                    FamilyHcid = hc.HierarchyClassParentId,
                    ClassHcid = hc.HierarchyClassId
                }),
                dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"UPDATE temp
                      SET SegmentHCID = hm.SegmentHCID
                  FROM #TempMerchandiseHierarchy temp
                  JOIN dbo.Hierarchy_Merchandise hm ON temp.FamilyHCID = hm.FamilyHCID", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"UPDATE hm
                      SET hm.ClassHCID = temp.ClassHCID
                  FROM dbo.Hierarchy_Merchandise hm
                  JOIN #TempMerchandiseHierarchy temp ON hm.FamilyHCID = temp.FamilyHCID
                      AND hm.ClassHCID IS NULL", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID)
                  SELECT temp.*
                  FROM #TempMerchandiseHierarchy temp
                  LEFT JOIN dbo.Hierarchy_Merchandise hm on temp.ClassHCID = hm.ClassHCID
                  WHERE hm.ClassHCID IS NULL", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"DROP TABLE #TempMerchandiseHierarchy", null, dbProvider.Transaction);
        }

        private void UpdateGs1Bricks(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                @"CREATE TABLE #TempMerchandiseHierarchy
                  (
                      SegmentHCID int null,
                      FamilyHCID int null,
                      ClassHCID int null,
                      BrickHCID int null,
                      SubBrickHCID int null
                  )", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"INSERT INTO #TempMerchandiseHierarchy 
                  VALUES (@SegmentHcid, @FamilyHcid, @ClassHcid, @BrickHcid, @SubBrickHcid)",
                data.HierarchyClasses.Select(hc => new MerchandiseHierarchyModel
                {
                    ClassHcid = hc.HierarchyClassParentId,
                    BrickHcid = hc.HierarchyClassId
                }),
                dbProvider.Transaction);

            dbProvider.Connection.Execute(
                @"UPDATE temp
                    SET SegmentHCID = hm.SegmentHCID,
                        FamilyHCID = hm.FamilyHCID
                  FROM #TempMerchandiseHierarchy temp
                  JOIN dbo.Hierarchy_Merchandise hm ON temp.ClassHCID = hm.ClassHCID

                  UPDATE hm
                      SET BrickHCID = temp.BrickHCID
                  FROM dbo.Hierarchy_Merchandise hm
                  JOIN #TempMerchandiseHierarchy temp ON hm.ClassHCID = temp.ClassHCID
                      AND hm.BrickHCID IS NULL

                  INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID)
                  SELECT temp.*
                  FROM #TempMerchandiseHierarchy temp
                  LEFT JOIN dbo.Hierarchy_Merchandise hm on temp.BrickHCID = hm.BrickHCID
                  WHERE hm.BrickHCID IS NULL", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"DROP TABLE #TempMerchandiseHierarchy", null, dbProvider.Transaction);
        }

        private void UpdateSubBricks(AddOrUpdateMerchandiseHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                @"CREATE TABLE #TempMerchandiseHierarchy
                  (
                      SegmentHCID int null,
                      FamilyHCID int null,
                      ClassHCID int null,
                      BrickHCID int null,
                      SubBrickHCID int null
                  )", null, dbProvider.Transaction);

            dbProvider.Connection.Execute(
                @"INSERT INTO #TempMerchandiseHierarchy 
                  VALUES (@SegmentHcid, @FamilyHcid, @ClassHcid, @BrickHcid, @SubBrickHcid)",
                data.HierarchyClasses.Select(hc => new MerchandiseHierarchyModel
                {
                    BrickHcid = hc.HierarchyClassParentId,
                    SubBrickHcid = hc.HierarchyClassId
                }),
                dbProvider.Transaction);

            dbProvider.Connection.Execute(
                @"UPDATE temp
                      SET SegmentHCID = hm.SegmentHCID,
                          FamilyHCID = hm.FamilyHCID,
                          ClassHCID = hm.ClassHCID
                  FROM #TempMerchandiseHierarchy temp
                  JOIN dbo.Hierarchy_Merchandise hm ON temp.BrickHCID = hm.BrickHCID

                  UPDATE hm
                      SET SubBrickHCID = temp.SubBrickHCID
                  FROM dbo.Hierarchy_Merchandise hm
                  JOIN #TempMerchandiseHierarchy temp ON hm.BrickHCID = temp.BrickHCID
                      AND hm.SubBrickHCID IS NULL

                  INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID)
                  SELECT temp.*
                  FROM #TempMerchandiseHierarchy temp
                  LEFT JOIN dbo.Hierarchy_Merchandise hm on temp.SubBrickHCID = hm.SubBrickHCID
                  WHERE hm.SubBrickHCID IS NULL", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"DROP TABLE #TempMerchandiseHierarchy", null, dbProvider.Transaction);
        }
    }
}
