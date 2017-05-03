IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'ItemAdd')
	BEGIN
		PRINT 'Dropping Trigger ItemAdd'
		DROP  Trigger ItemAdd
	END
GO


PRINT 'Creating Trigger ItemAdd'
GO
CREATE Trigger ItemAdd 
ON Item
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    -- Queue for Price Modeling if necessary
    INSERT INTO PMProductChg (HierLevel, Item_Key, ItemDescription, ParentID, ParentDescription, ActionID, Status)
    SELECT 'Product', Inserted.Item_Key, Inserted.Item_Description, 
           ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Inserted.SubTeam_No) + '1'), ISNULL(Category_Name, 'NO CATEGORY'), 
           'ADD', 'ACTIVE'
    FROM Inserted
    LEFT JOIN
        ItemCategory
        ON Inserted.Category_ID = ItemCategory.Category_ID
    WHERE Inserted.Retail_Sale = 1
          AND (Inserted.SubTeam_No IN (SELECT SubTeam_No FROM PMSubTeamInclude (nolock)))

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

