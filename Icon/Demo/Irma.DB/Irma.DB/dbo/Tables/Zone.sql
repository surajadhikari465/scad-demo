CREATE TABLE [dbo].[Zone] (
    [Zone_ID]                INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Zone_Name]              VARCHAR (100) NULL,
    [Region_ID]              INT           NULL,
    [GLMarketingExpenseAcct] INT           NULL,
    [LastUpdate]             ROWVERSION    NOT NULL,
    [LastUpdateUserID]       INT           NULL,
    CONSTRAINT [PK_Zone_Zone_ID] PRIMARY KEY CLUSTERED ([Zone_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Region_Region_ID] FOREIGN KEY ([Region_ID]) REFERENCES [dbo].[Region] ([Region_ID])
);





GO
ALTER TABLE [dbo].[Zone] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER ZoneAddUpdate
ON Zone
FOR INSERT, UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	INSERT INTO PMOrganizationChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'Zone', Inserted.Zone_ID, Inserted.Zone_Name, Region.Region_ID, RegionName,
           CASE WHEN (Deleted.Zone_ID IS NULL) OR (Inserted.Region_ID IS NOT NULL AND Deleted.Region_ID IS NULL)
                     THEN 'ADD'
                ELSE 'CHANGE' END
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Inserted.Zone_ID = Deleted.Zone_ID
    LEFT JOIN
        Region
        ON ISNULL(Inserted.Region_ID, Deleted.Region_ID) = Region.Region_ID
    WHERE ((ISNULL(Deleted.Zone_Name, '') <> ISNULL(Inserted.Zone_Name, ''))
           OR (ISNULL(Deleted.Region_ID, 0) <> ISNULL(Inserted.Region_ID, 0)))
          AND (Deleted.Region_ID IS NOT NULL OR Inserted.Region_ID IS NOT NULL)

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ZoneAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT DELETE
    ON OBJECT::[dbo].[Zone] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Zone] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Zone] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Zone] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Zone] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Zone] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Zone] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Zone] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Zone] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Zone] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Zone] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Zone] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Zone] TO [ExtractRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Zone] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Zone] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Zone] TO [iCONReportingRole]
    AS [dbo];

