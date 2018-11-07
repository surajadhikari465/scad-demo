CREATE TABLE [dbo].[Scale_Allergen] (
    [Scale_Allergen_ID]  INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Scale_LabelType_ID] INT            NULL,
    [Description]        VARCHAR (50)   NOT NULL,
    [Allergens]          VARCHAR (4200) NOT NULL,
    PRIMARY KEY CLUSTERED ([Scale_Allergen_ID] ASC) WITH (FILLFACTOR = 80),
    FOREIGN KEY ([Scale_LabelType_ID]) REFERENCES [dbo].[Scale_LabelType] ([Scale_LabelType_ID])
);


GO
ALTER TABLE [dbo].[Scale_Allergen] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO

CREATE Trigger Scale_Allergen_Update
ON [dbo].[Scale_Allergen]
FOR UPDATE
AS
-- **************************************************************************
-- Trigger: Scale_Allergen_Update()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This trigger is called to put nutrifact records in the queue to be sent to the scales.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 07/13/2016   MZ       20166(15920)   Created the trigger
-- **************************************************************************
BEGIN
    DECLARE @Error_No int 
    SELECT @Error_No = 0

	-- Queue for PlumCorpChgQueue
	BEGIN
		INSERT INTO 
		        PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
            SELECT 
		        isc.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
            INNER JOIN 
				Deleted ON Deleted.Scale_Allergen_ID = Inserted.Scale_Allergen_ID
			INNER JOIN ItemScale isc ON isc.Scale_Allergen_ID = Inserted.Scale_Allergen_ID
            CROSS JOIN 
				Store s
            JOIN 
				StoreItem si ON si.Item_Key = isc.Item_Key AND si.Store_No = s.Store_No
			WHERE 
				(s.WFM_Store = 1 OR s.Mega_Store = 1)
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = isc.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = isc.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
				AND NOT EXISTS (SELECT icfs.Item_Key from ItemCustomerFacingScale icfs (NOLOCK) WHERE icfs.Item_Key = si.Item_Key)
			UNION
			SELECT 
		        isc.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
            INNER JOIN 
				Deleted ON Deleted.Scale_Allergen_ID = Inserted.Scale_Allergen_ID
			INNER JOIN ItemScale isc ON isc.Scale_Allergen_ID = Inserted.Scale_Allergen_ID
            CROSS JOIN 
				Store s
            INNER JOIN 
				StoreItem si ON si.Item_Key = isc.Item_Key AND si.Store_No = s.Store_No
			INNER JOIN
				ItemCustomerFacingScale icfs on icfs.Item_Key = si.Item_Key and icfs.SendToScale = 1
			WHERE
				s.Mega_Store = 1
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = isc.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = isc.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
			SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_Ingredient_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRSUser]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [iCONReportingRole]
    AS [dbo];

