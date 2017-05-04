CREATE TABLE [dbo].[Scale_ExtraText] (
    [Scale_ExtraText_ID] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Scale_LabelType_ID] INT            NOT NULL,
    [Description]        VARCHAR (50)   NOT NULL,
    [ExtraText]          VARCHAR (4200) NOT NULL,
    CONSTRAINT [PK_ExtraText] PRIMARY KEY CLUSTERED ([Scale_ExtraText_ID] ASC),
    CONSTRAINT [FK_Scale_ExtraText_Scale_LabelType] FOREIGN KEY ([Scale_LabelType_ID]) REFERENCES [dbo].[Scale_LabelType] ([Scale_LabelType_ID])
);


GO
ALTER TABLE [dbo].[Scale_ExtraText] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [Index_Description]
    ON [dbo].[Scale_ExtraText]([Description] ASC);


GO
CREATE Trigger Scale_ExtraText_Add 
ON [dbo].[Scale_ExtraText]
FOR INSERT
-- **************************************************************************
-- Trigger: Scale_ExtraText_Add()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This trigger is called to put extra text records in the queue to be sent to the scales.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 8/24/2011	MZ				2665	Pulled records from the ItemScaleOverride talbe also for secondary jurisdiction when 
--										inserting records into the PLUMCorpChgQueue table.
-- 06/05/2012	BJL   			6583	Added check against the Scale_ExtraTextChgQueue and
--										Scale_ExtraTextChgQueueTmp tables to prevent duplicate entry.
-- **************************************************************************
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	-- DETERMINE IF CURRENT REGION PUSHES SCALE EXTRA TEXT DATA --
    DECLARE @PushScaleExtraTextData bit
    SELECT @PushScaleExtraTextData = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'PushScaleExtraTextData'
           
    IF @Error_No = 0 AND @PushScaleExtraTextData = 1
	BEGIN
		-- Queue for ExtraTextChg
		INSERT INTO 
			Scale_ExtraTextChgQueue (Scale_ExtraText_ID, ActionCode)
		SELECT 
			Inserted.Scale_ExtraText_ID, 'A'
		FROM 
			Inserted
		WHERE
			NOT EXISTS (SELECT 1 FROM Scale_ExtraTextChgQueue SQ WHERE SQ.Scale_ExtraText_ID = Inserted.Scale_ExtraText_ID)
		AND NOT EXISTS (SELECT 1 FROM Scale_ExtraTextChgQueueTmp SQT WHERE SQT.Scale_ExtraText_ID = Inserted.Scale_ExtraText_ID)
		
		SELECT @Error_No = @@ERROR
	END    

	IF @Error_No = 0
	BEGIN
		-- The PLUM scales do not query the extra text queue.  All scale items that are associated with this
		-- extra text record need to be added to the PLUMCorpChgQueue.
		INSERT INTO 
			PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		(SELECT 
			ItemScale.Item_Key, 'C', s.Store_No
		FROM 
			Inserted 
		INNER JOIN 
			ItemScale ON Inserted.Scale_ExtraText_ID = ItemScale.Scale_ExtraText_Id
		CROSS JOIN
			Store s
		WHERE
			ItemScale.Item_Key NOT IN (SELECT Queue.Item_Key FROM PLUMCorpChgQueue Queue)
		UNION
		SELECT 
			ItemScaleOverride.Item_Key, 'C', s.Store_No
		FROM 
			Inserted 
		INNER JOIN 
			ItemScaleOverride ON Inserted.Scale_ExtraText_ID = ItemScaleOverride.Scale_ExtraText_Id
		CROSS JOIN
			Store s
		WHERE
			ItemScaleOverride.Item_Key NOT IN (SELECT Queue.Item_Key FROM PLUMCorpChgQueue Queue))

		SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_ExtraText_Add trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
CREATE Trigger Scale_ExtraText_Update 
ON [dbo].[Scale_ExtraText]
FOR UPDATE
AS
-- **************************************************************************
-- Trigger: Scale_ExtraText_Update()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This trigger is called to put extra text records in the queue to be sent to the scales.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 8/24/2011	MZ				2665	Pulled records from the ItemScaleOverride talbe also for secondary jurisdiction when 
--										inserting records into the PLUMCorpChgQueue table.
-- 06/05/2012	BJL   			6583	Added check against the Scale_ExtraTextChgQueue and
--										Scale_ExtraTextChgQueueTmp tables to prevent duplicate entry.
-- **************************************************************************
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	-- DETERMINE IF CURRENT REGION PUSHES SCALE EXTRA TEXT DATA --
    DECLARE @PushScaleExtraTextData bit
    SELECT @PushScaleExtraTextData = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'PushScaleExtraTextData'
           
    IF @Error_No = 0 AND @PushScaleExtraTextData = 1
	BEGIN	
		-- Queue for ExtraTextChg
		INSERT INTO 
			Scale_ExtraTextChgQueue (Scale_ExtraText_ID, ActionCode)
		SELECT 
			Inserted.Scale_ExtraText_ID, 'C'
		FROM 
			Inserted
		WHERE
			NOT EXISTS (SELECT 1 FROM Scale_ExtraTextChgQueue SQ WHERE SQ.Scale_ExtraText_ID = Inserted.Scale_ExtraText_ID)
		AND NOT EXISTS (SELECT 1 FROM Scale_ExtraTextChgQueueTmp SQT WHERE SQT.Scale_ExtraText_ID = Inserted.Scale_ExtraText_ID)
		
		SELECT @Error_No = @@ERROR
	END
    
    IF @Error_No = 0
    BEGIN
		-- The PLUM scales do not query the extra text queue.  All scale items that are associated with this
		-- extra text record need to be added to the PLUMCorpChgQueue.
		INSERT INTO 
			PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		(SELECT 
			ItemScale.Item_Key, 'C', s.Store_No
		FROM ItemScale, Inserted  
		CROSS JOIN
			Store s
		WHERE
			ItemScale.Scale_ExtraText_Id = Inserted.Scale_ExtraText_ID 
			AND ItemScale.Item_Key NOT IN (SELECT Queue.Item_Key FROM PLUMCorpChgQueue Queue)
		UNION
		SELECT 
			ItemScaleOverride.Item_Key, 'C', s.Store_No
		FROM ItemScaleOverride, Inserted 
		CROSS JOIN
			Store s
		WHERE
			ItemScaleOverride.Scale_ExtraText_Id = Inserted.Scale_ExtraText_ID  
			AND ItemScaleOverride.Item_Key NOT IN (SELECT Queue.Item_Key FROM PLUMCorpChgQueue Queue))
	
		SELECT @Error_No = @@ERROR
	END
	
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_ExtraText_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMAReports]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_ExtraText] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_ExtraText] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_ExtraText] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_ExtraText] TO [iCONReportingRole]
    AS [dbo];

