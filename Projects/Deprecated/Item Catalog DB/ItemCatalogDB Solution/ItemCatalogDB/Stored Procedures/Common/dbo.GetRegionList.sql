 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.GetRegionList') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.GetRegionList
GO

CREATE PROCEDURE dbo.GetRegionList
AS

BEGIN
    SET NOCOUNT ON

    SELECT DISTINCT Region_Code 
    FROM StoreRegionMapping

    SET NOCOUNT OFF
END
GO 