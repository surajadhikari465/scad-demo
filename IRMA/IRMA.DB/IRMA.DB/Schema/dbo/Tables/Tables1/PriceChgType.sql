CREATE TABLE [dbo].[PriceChgType] (
    [PriceChgTypeID]      TINYINT      NOT NULL,
    [PriceChgTypeDesc]    VARCHAR (20) NOT NULL,
    [Priority]            SMALLINT     NOT NULL,
    [On_Sale]             BIT          CONSTRAINT [DF_PriceChgType_On_Sale] DEFAULT ((1)) NOT NULL,
    [MSRP_Required]       BIT          CONSTRAINT [DF_PriceChgType_MSRP_Required] DEFAULT ((0)) NOT NULL,
    [LineDrive]           BIT          CONSTRAINT [DF_PriceChgType_LineDrive] DEFAULT ((0)) NOT NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    [Competitive]         BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PriceChgType] PRIMARY KEY CLUSTERED ([PriceChgTypeID] ASC),
    CONSTRAINT [UQ_PriceChgType_Priority] UNIQUE NONCLUSTERED ([Priority] ASC)
);


GO
ALTER TABLE [dbo].[PriceChgType] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER [dbo].[PriceChgTypeAddUpdate] ON [dbo].[PriceChgType] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update PriceChgType 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where PriceChgType.PriceChgTypeId = i.PriceChgTypeId
	 
    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PriceChgTypeAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceChgType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[PriceChgType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[PriceChgType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceChgType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceChgType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[PriceChgType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceChgType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[PriceChgType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[PriceChgType] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceChgType] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceChgType] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[PriceChgType] TO [IRMA_Teradata]
    AS [dbo];





GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[PriceChgType] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[PriceChgType] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceChgType] TO [TibcoDataWriter]
    AS [dbo];