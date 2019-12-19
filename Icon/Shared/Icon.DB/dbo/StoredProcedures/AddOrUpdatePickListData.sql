CREATE PROCEDURE [dbo].[AddOrUpdatePickListData] @PickList dbo.PickListType READONLY
AS
BEGIN
	CREATE TABLE #PickList(
		PickListId INT NOT NULL,
		AttributeId INT NULL,
		PickListValue NVARCHAR(50) NULL);

	INSERT INTO #PickList (PickListId, AttributeId, PickListValue)
	SELECT DISTINCT PickListId, AttributeId, PickListValue
	FROM @PickList;

    DELETE pld
    FROM dbo.PickListData pld
    WHERE pld.AttributeId IN(SELECT DISTINCT AttributeId FROM #PickList)
      AND pld.PickListId NOT IN(SELECT DISTINCT PickListId FROM #PickList);

    MERGE dbo.PickListData WITH(UPDLOCK, ROWLOCK) pld
    USING #PickList pl ON pld.picklistId = pl.picklistId
    
    WHEN NOT MATCHED THEN
      INSERT(AttributeId, PickListValue)
      VALUES(pl.AttributeId, pl.PickListValue)
    
    WHEN MATCHED AND pld.PickListValue <> pl.PickListValue
    THEN
      UPDATE SET PickListValue = pl.PickListValue;
END