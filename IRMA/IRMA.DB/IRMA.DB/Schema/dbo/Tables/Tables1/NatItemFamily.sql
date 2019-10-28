CREATE TABLE [dbo].[NatItemFamily] (
    [NatFamilyID]         INT IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [NatFamilyName]       VARCHAR (65) NULL,
    [NatSubTeam_No]       INT          NULL,
    [SubTeam_No]          INT          NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_NatItemFamily] PRIMARY KEY CLUSTERED ([NatFamilyID] ASC)
);


GO
ALTER TABLE [dbo].[NatItemFamily] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER [dbo].[NatItemFamilyAddUpdate] ON [dbo].[NatItemFamily] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update NatItemFamily
		Set LastUpdateTimestamp = GetDate()
	from Inserted I
	where NatItemFamily.NatFamilyId = i.NatFamilyId

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('NatItemFamilyAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemFamily] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemFamily] TO [ExtractRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemFamily] TO [IRMA_Teradata]
    AS [dbo];





GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemFamily] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemFamily] TO [iCONReportingRole]
    AS [dbo];

