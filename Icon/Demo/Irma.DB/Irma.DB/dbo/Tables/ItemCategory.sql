CREATE TABLE [dbo].[ItemCategory] (
    [Category_ID]     INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Category_Name]   VARCHAR (35) NOT NULL,
    [SubTeam_No]      INT          NULL,
    [User_ID]         INT          NULL,
    [SubTeam_Type_ID] INT          NULL,
    CONSTRAINT [PK_ItemCategory_Category_ID] PRIMARY KEY CLUSTERED ([Category_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemCategory_1__13] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_ItemCategory_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);





GO
ALTER TABLE [dbo].[ItemCategory] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxItemCategoryName]
    ON [dbo].[ItemCategory]([Category_Name] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemCategoryUserID]
    ON [dbo].[ItemCategory]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemCategorySubTeam]
    ON [dbo].[ItemCategory]([SubTeam_No] ASC) WITH (FILLFACTOR = 80);


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
GRANT DELETE
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemCategory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCategory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemCategory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCategory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCategory] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemCategory] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemCategory] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCategory] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemCategory] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemCategory] TO [iCONReportingRole]
    AS [dbo];

