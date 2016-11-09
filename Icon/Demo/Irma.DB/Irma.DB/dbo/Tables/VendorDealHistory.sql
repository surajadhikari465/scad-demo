CREATE TABLE [dbo].[VendorDealHistory] (
    [VendorDealHistoryID] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [StoreItemVendorID]   INT            NOT NULL,
    [CaseQty]             INT            NOT NULL,
    [Package_Desc1]       DECIMAL (9, 4) NOT NULL,
    [CaseAmt]             SMALLMONEY     NOT NULL,
    [StartDate]           SMALLDATETIME  NOT NULL,
    [EndDate]             SMALLDATETIME  NOT NULL,
    [VendorDealTypeID]    INT            NOT NULL,
    [FromVendor]          BIT            CONSTRAINT [DF_VendorDealHistory_FromVendor] DEFAULT ((0)) NOT NULL,
    [InsertDate]          DATETIME       CONSTRAINT [DF_VendorDealHistory_InsertDate] DEFAULT (getdate()) NOT NULL,
    [InsertWorkStation]   VARCHAR (255)  CONSTRAINT [DF_VendorDealHistory_InsertWorkStation] DEFAULT (host_name()) NOT NULL,
    [CostPromoCodeTypeID] INT            NULL,
    [NotStackable]        BIT            CONSTRAINT [DF_VendorDealHistory_NotStackable] DEFAULT ((0)) NOT NULL,
    [LastUpdateTimestamp] DATETIME       NULL,
    CONSTRAINT [PK_VendorDealHistory] PRIMARY KEY CLUSTERED ([VendorDealHistoryID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_VendorDealHistory_CostPromoCodeTypeID] FOREIGN KEY ([CostPromoCodeTypeID]) REFERENCES [dbo].[CostPromoCodeType] ([CostPromoCodeTypeID]),
    CONSTRAINT [FK_VendorDealHistory_StoreItemVendor] FOREIGN KEY ([StoreItemVendorID]) REFERENCES [dbo].[StoreItemVendor] ([StoreItemVendorID]),
    CONSTRAINT [FK_VendorDealHistory_VendorDealType] FOREIGN KEY ([VendorDealTypeID]) REFERENCES [dbo].[VendorDealType] ([VendorDealTypeID])
);





GO
ALTER TABLE [dbo].[VendorDealHistory] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_VendorDealHistorySIV]
    ON [dbo].[VendorDealHistory]([StoreItemVendorID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_VendorDealHistorySIVStartEndDates]
    ON [dbo].[VendorDealHistory]([StoreItemVendorID] ASC, [StartDate] ASC, [EndDate] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_VendorDealHistoryDealType]
    ON [dbo].[VendorDealHistory]([VendorDealTypeID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER [dbo].[VendorDealHistoryAddUpdate] ON [dbo].[VendorDealHistory] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    UPDATE VendorDealHistory
		SET LastUpdateTimestamp = GetDate()
	FROM Inserted i
	WHERE VendorDealHistory.VendorDealHistoryId = i.VendorDealHistoryId

	UPDATE StoreItemVendor
		SET LastCostAddedDate = GETDATE()
		FROM StoreItemVendor SIV
		INNER JOIN Inserted INS ON SIV.StoreItemVendorID = INS.StoreItemVendorID    

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('VendorDealHistoryAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealHistory] TO [IMHARole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealHistory] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealHistory] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealHistory] TO [iCONReportingRole]
    AS [dbo];

