CREATE TABLE [dbo].[Region] (
    [Region_ID]             INT           IDENTITY (1, 1) NOT NULL,
    [RegionName]            VARCHAR (100) NULL,
    [RegionCode]            VARCHAR (4)   NULL,
    [CentralTimeZoneOffset] INT           NULL,
    CONSTRAINT [PK_Region_Region_ID] PRIMARY KEY CLUSTERED ([Region_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE TRIGGER RegionAddUpdate
ON [dbo].[Region]
FOR INSERT, UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	INSERT INTO PMOrganizationChg (HierLevel, ItemID, ItemDescription, ActionID)
    SELECT 'Region', CONVERT(varchar(255), Inserted.Region_ID), Inserted.RegionName,
           CASE WHEN Deleted.Region_ID IS NULL THEN 'ADD' ELSE 'CHANGE' END
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Inserted.Region_ID = Deleted.Region_ID
    WHERE ISNULL(Deleted.RegionName, '') <> ISNULL(Inserted.RegionName, '')

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('RegionAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT DELETE
    ON OBJECT::[dbo].[Region] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Region] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Region] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Region] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Region] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Region] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Region] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Region] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Region] TO [WFM.R10.Operations.IRMAPriceAudit]
    AS [dbo];

