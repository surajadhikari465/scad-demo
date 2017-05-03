IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'ItemDelete' 
	   AND 	  type = 'TR')
    DROP TRIGGER ItemDelete
GO

CREATE TRIGGER ItemDelete
ON Item
FOR DELETE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	INSERT INTO PMProductChg (HierLevel, Item_Key, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'Product', Deleted.Item_Key, Identifier, Item_Description, 
           ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Deleted.SubTeam_No) + '1'), ISNULL(Category_Name, 'NO CATEGORY'), 
           'DELETE'
    FROM Deleted
        LEFT JOIN
            ItemIdentifier II
            ON Deleted.Item_Key = II.Item_Key AND Default_Identifier = 1
        LEFT JOIN
            ItemCategory
            ON Deleted.Category_ID = ItemCategory.Category_ID
    WHERE Deleted.Retail_Sale = 1
          AND Deleted.SubTeam_No IN (SELECT SubTeam_No FROM PMSubTeamInclude (nolock))
          AND NOT EXISTS (SELECT * FROM PMExcludedItem WHERE Item_Key = Deleted.Item_Key)

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

