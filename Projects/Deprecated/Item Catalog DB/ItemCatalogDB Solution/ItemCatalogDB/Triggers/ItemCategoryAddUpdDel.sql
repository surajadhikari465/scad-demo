IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'ItemCategoryAddUpdDel' 
	   AND 	  type = 'TR')
    DROP TRIGGER ItemCategoryAddUpdDel
GO

CREATE TRIGGER ItemCategoryAddUpdDel
ON ItemCategory
FOR INSERT, UPDATE, DELETE
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    -- Category Adds and Changes
    INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'Category', CONVERT(varchar(255), Inserted.Category_ID), Inserted.Category_Name, SubTeam.SubTeam_No, SubTeam.SubTeam_Name,
           CASE WHEN Deleted.Category_ID IS NULL THEN 'ADD' ELSE 'CHANGE' END
    FROM Inserted
        LEFT JOIN
            SubTeam
            ON Inserted.SubTeam_No = SubTeam.SubTeam_No
        LEFT JOIN
            Deleted
            ON Inserted.Category_ID = Deleted.Category_ID
    WHERE (ISNULL(Deleted.Category_Name, '') <> ISNULL(Inserted.Category_Name, ''))
           OR (ISNULL(Deleted.SubTeam_No, 0) <> ISNULL(Inserted.SubTeam_No, 0))

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        -- Sub-Category - repeat Category with Category as the parent
        INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
        SELECT 'SubCategory', CONVERT(varchar(255), Inserted.Category_ID), Inserted.Category_Name, CONVERT(varchar(255), Inserted.Category_ID), Inserted.Category_Name,
               CASE WHEN Deleted.Category_ID IS NULL THEN 'ADD' ELSE 'CHANGE' END
        FROM Inserted
            LEFT JOIN
                Deleted
                ON Inserted.Category_ID = Deleted.Category_ID
        WHERE (ISNULL(Deleted.Category_Name, '') <> ISNULL(Inserted.Category_Name, ''))
               OR (ISNULL(Deleted.SubTeam_No, 0) <> ISNULL(Inserted.SubTeam_No, 0))

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        -- Deletes - Sub-Category
        INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
        SELECT 'SubCategory', CONVERT(varchar(255), Deleted.Category_ID), Deleted.Category_Name, CONVERT(varchar(255), Deleted.Category_ID), Deleted.Category_Name,
               'DELETE'
        FROM Deleted
            LEFT JOIN
                Inserted
                ON Deleted.Category_ID = Inserted.Category_ID
        WHERE Inserted.Category_ID IS NULL

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        -- Deletes - Category
        INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
        SELECT 'Category', CONVERT(varchar(255), Deleted.Category_ID), Deleted.Category_Name, SubTeam.SubTeam_No, SubTeam.SubTeam_Name,
               'DELETE'
        FROM Deleted
            LEFT JOIN
                Inserted
                ON Deleted.Category_ID = Inserted.Category_ID
            LEFT JOIN
                SubTeam
                ON Deleted.SubTeam_No = SubTeam.SubTeam_No
        WHERE Inserted.Category_ID IS NULL

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemCategoryAddUpdDel trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

