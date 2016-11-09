CREATE TABLE [dbo].[AvgCostHistory] (
    [Item_Key]            INT             NOT NULL,
    [Store_No]            INT             NOT NULL,
    [SubTeam_No]          INT             NOT NULL,
    [Effective_Date]      DATETIME        CONSTRAINT [DF_AvgCostHistory_Effective_Date] DEFAULT (getdate()) NOT NULL,
    [AvgCost]             SMALLMONEY      NOT NULL,
    [Quantity]            DECIMAL (18, 4) NULL,
    [Reason]              INT             NULL,
    [Comments]            VARCHAR (1000)  NULL,
    [User_ID]             INT             NULL,
    [LastUpdateTimestamp] DATETIME        NULL,
    [AvgCostHistoryId]    INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    CONSTRAINT [pk_AvgCostHistory_AvgCostHistoryId] PRIMARY KEY CLUSTERED ([AvgCostHistoryId] ASC, [Effective_Date] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_AvgCostHistory_AvgCostAdjReason] FOREIGN KEY ([Reason]) REFERENCES [dbo].[AvgCostAdjReason] ([ID])
);





GO
ALTER TABLE [dbo].[AvgCostHistory] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_AvgCostHistory]
    ON [dbo].[AvgCostHistory]([Item_Key] ASC, [Store_No] ASC, [SubTeam_No] ASC, [Effective_Date] ASC) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER [dbo].[AvgCostHistoryAddUpdate] ON [dbo].[AvgCostHistory] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update	
			AvgCostHistory 
		Set 
			LastUpdateTimestamp = GetDate()
	from	
			Inserted i 
	where 
				AvgCostHistory.Item_Key = i.Item_Key 
			AND AvgCostHistory.Store_No = i.Store_No 
			AND AvgCostHistory.SubTeam_No = i.SubTeam_No 
			AND AvgCostHistory.Effective_Date = i.Effective_Date

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('AvgCostHistoryAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AvgCostHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AvgCostHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AvgCostHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AvgCostHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AvgCostHistory] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AvgCostHistory] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AvgCostHistory] TO [iCONReportingRole]
    AS [dbo];

