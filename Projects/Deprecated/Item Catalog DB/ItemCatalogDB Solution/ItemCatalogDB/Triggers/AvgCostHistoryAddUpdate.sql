
-- AvgCostHistory
IF EXISTS ( SELECT *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'AvgCostHistoryAddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[AvgCostHistoryAddUpdate]
   END
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