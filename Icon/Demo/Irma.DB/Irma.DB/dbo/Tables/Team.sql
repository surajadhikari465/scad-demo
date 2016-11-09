CREATE TABLE [dbo].[Team] (
    [Team_No]           INT           NOT NULL,
    [Team_Name]         VARCHAR (100) NULL,
    [Team_Abbreviation] VARCHAR (10)  NULL,
    CONSTRAINT [PK_Team_Team_No] PRIMARY KEY CLUSTERED ([Team_No] ASC) WITH (FILLFACTOR = 80)
);





GO
ALTER TABLE [dbo].[Team] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER TeamAddUpdate
ON Team
FOR INSERT, UPDATE
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ActionID)
    SELECT 'Team', CONVERT(varchar(255), Inserted.Team_No), Inserted.Team_Name,
           CASE WHEN Deleted.Team_No IS NULL THEN 'ADD' ELSE 'CHANGE' END
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Inserted.Team_No = Deleted.Team_No
    WHERE ISNULL(Deleted.Team_Name, '') <> ISNULL(Inserted.Team_Name, '')

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('TeamAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT DELETE
    ON OBJECT::[dbo].[Team] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Team] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Team] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Team] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Team] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Team] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Team] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Team] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Team] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Team] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Team] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Team] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Team] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Team] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Team] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Team] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Team] TO [iCONReportingRole]
    AS [dbo];

