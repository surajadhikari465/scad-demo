CREATE TABLE [dbo].[NatItemCat] (
	[NatCatID]            INT IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [NatCatName]          VARCHAR (65) NULL,
    [NatFamilyID]         INT          NOT NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_NatItemCat] PRIMARY KEY CLUSTERED ([NatCatID] ASC)
);


GO
ALTER TABLE [dbo].[NatItemCat] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER [dbo].[NatItemCatAddUpdate] ON [dbo].[NatItemCat] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update NatItemCat 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where NatItemCat.NatCatId = i.NatCatId

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('NatItemCatAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemCat] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemCat] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemCat] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemCat] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemCat] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemCat] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemCat] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemCat] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemCat] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemCat] TO [ExtractRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatItemCat] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemCat] TO [IRMA_Teradata]
    AS [dbo];





GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemCat] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NatItemCat] TO [iCONReportingRole]
    AS [dbo];

