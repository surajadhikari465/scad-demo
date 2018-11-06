CREATE TABLE [dbo].[ItemScaleOverride] (
    [Item_Key]                  INT          NOT NULL,
    [StoreJurisdictionID]       INT          NOT NULL,
    [Scale_Description1]        VARCHAR (64) NULL,
    [Scale_Description2]        VARCHAR (64) NULL,
    [Scale_Description3]        VARCHAR (64) NULL,
    [Scale_Description4]        VARCHAR (64) NULL,
    [Scale_ExtraText_ID]        INT          NULL,
    [Scale_Tare_ID]             INT          NULL,
    [Scale_LabelStyle_ID]       INT          NULL,
    [Scale_ScaleUOMUnit_ID]     INT          NULL,
    [Scale_RandomWeightType_ID] INT          NULL,
    [Scale_FixedWeight]         VARCHAR (25) NULL,
    [Scale_ByCount]             INT          NULL,
    [ShelfLife_Length]          SMALLINT     NULL,
    [ForceTare]                 BIT          CONSTRAINT [DF_ItemScaleOverride_ForceTare] DEFAULT ((0)) NOT NULL,
    [Scale_Alternate_Tare_ID]   INT          NULL,
    [Scale_EatBy_ID]            INT          NULL,
    [Scale_Grade_ID]            INT          NULL,
    [PrintBlankShelfLife]       BIT          CONSTRAINT [DF_ItemScaleOverride_PrintBlankShelfLife] DEFAULT ((0)) NOT NULL,
    [PrintBlankEatBy]           BIT          CONSTRAINT [DF_ItemScaleOverride_PrintBlankEatBy] DEFAULT ((0)) NOT NULL,
    [PrintBlankPackDate]        BIT          CONSTRAINT [DF_ItemScaleOverride_PrintBlankPackDate] DEFAULT ((0)) NOT NULL,
    [PrintBlankWeight]          BIT          CONSTRAINT [DF_ItemScaleOverride_PrintBlankWeight] DEFAULT ((0)) NOT NULL,
    [PrintBlankUnitPrice]       BIT          CONSTRAINT [DF_ItemScaleOverride_PrintBlankUnitPrice] DEFAULT ((0)) NOT NULL,
    [PrintBlankTotalPrice]      BIT          CONSTRAINT [DF_ItemScaleOverride_PrintBlankTotalPrice] DEFAULT ((0)) NOT NULL,
    [Nutrifact_ID]              INT          NULL,
    CONSTRAINT [PK_ItemScaleOverride] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [StoreJurisdictionID] ASC),
    CONSTRAINT [FK_ItemScaleOverride_ExtraText_ID] FOREIGN KEY ([Scale_ExtraText_ID]) REFERENCES [dbo].[Scale_ExtraText] ([Scale_ExtraText_ID]),
    CONSTRAINT [FK_ItemScaleOverride_Item_Key] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemScaleOverride_NutriFact] FOREIGN KEY ([Nutrifact_ID]) REFERENCES [dbo].[NutriFacts] ([NutriFactsID]),
    CONSTRAINT [FK_ItemScaleOverride_Scale_Alternate_Tare_ID] FOREIGN KEY ([Scale_Alternate_Tare_ID]) REFERENCES [dbo].[Scale_Tare] ([Scale_Tare_ID]),
    CONSTRAINT [FK_ItemScaleOverride_Scale_EatBy] FOREIGN KEY ([Scale_EatBy_ID]) REFERENCES [dbo].[Scale_EatBy] ([Scale_EatBy_ID]),
    CONSTRAINT [FK_ItemScaleOverride_Scale_Grade] FOREIGN KEY ([Scale_Grade_ID]) REFERENCES [dbo].[Scale_Grade] ([Scale_Grade_ID]),
    CONSTRAINT [FK_ItemScaleOverride_Scale_LabelStyle_ID] FOREIGN KEY ([Scale_LabelStyle_ID]) REFERENCES [dbo].[Scale_LabelStyle] ([Scale_LabelStyle_ID]),
    CONSTRAINT [FK_ItemScaleOverride_Scale_RandomWeightType_ID] FOREIGN KEY ([Scale_RandomWeightType_ID]) REFERENCES [dbo].[Scale_RandomWeightType] ([Scale_RandomWeightType_ID]),
    CONSTRAINT [FK_ItemScaleOverride_Scale_Tare_ID] FOREIGN KEY ([Scale_Tare_ID]) REFERENCES [dbo].[Scale_Tare] ([Scale_Tare_ID]),
    CONSTRAINT [FK_ItemScaleOverride_ScaleUOMUnit_ID] FOREIGN KEY ([Scale_ScaleUOMUnit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemScaleOverride_StoreJurisdictionID] FOREIGN KEY ([StoreJurisdictionID]) REFERENCES [dbo].[StoreJurisdiction] ([StoreJurisdictionID])
);


GO
ALTER TABLE [dbo].[ItemScaleOverride] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE Trigger Scale_ItemScaleOverrideUpdate ON [dbo].[ItemScaleOverride] 
FOR UPDATE 

AS 

-- ****************************************************************************************************************
-- Procedure: Scale_ItemScaleOverrideUpdate()
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-25	KM		9382	Add update history template; account for all new ItemScaleOverride values in 4.8;
-- 2013-01-31	KM		9393	Include Nutrifact_ID;
-- 2013-02-06   MZ      9392    Added a where statement to restrict PLUMCorpChgQueue records to be created only for 
--                              stores in the same jurisdiction.
-- ****************************************************************************************************************

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    IF @error_no = 0
    BEGIN
		-- CREATE A HISTORY RECORD TO TRACK THE CHANGE
		INSERT INTO ItemScaleOverrideHistory (
			Item_Key, 
			StoreJurisdictionID,
			Scale_Description1,
			Scale_Description2,
			Scale_Description3,
			Scale_Description4,
			Scale_ExtraText_ID,
			Scale_Tare_ID,
			Scale_LabelStyle_ID,
			Scale_ScaleUOMUnit_ID,
			Scale_RandomWeightType_ID,
			Scale_FixedWeight,
			Scale_ByCount,
			ShelfLife_Length,
			ForceTare,
			Scale_Alternate_Tare_ID,
			Scale_EatBy_ID,
			Scale_Grade_ID,
			PrintBlankEatBy,
			PrintBlankPackDate,
			PrintBlankShelfLife,
			PrintBlankTotalPrice,
			PrintBlankUnitPrice,
			PrintBlankWeight,
			Nutrifact_ID									
		) SELECT	
			Inserted.Item_Key, 
			Inserted.StoreJurisdictionID,
			Inserted.Scale_Description1,
			Inserted.Scale_Description2,
			Inserted.Scale_Description3,
			Inserted.Scale_Description4,
			Inserted.Scale_ExtraText_ID,
			Inserted.Scale_Tare_ID,
			Inserted.Scale_LabelStyle_ID,
			Inserted.Scale_ScaleUOMUnit_ID,
			Inserted.Scale_RandomWeightType_ID,
			Inserted.Scale_FixedWeight,
			Inserted.Scale_ByCount,
			Inserted.ShelfLife_Length,
			Inserted.ForceTare,
			Inserted.Scale_Alternate_Tare_ID,
			Inserted.Scale_EatBy_ID,
			Inserted.Scale_Grade_ID,
			Inserted.PrintBlankEatBy,
			Inserted.PrintBlankPackDate,
			Inserted.PrintBlankShelfLife,
			Inserted.PrintBlankTotalPrice,
			Inserted.PrintBlankUnitPrice,
			Inserted.PrintBlankWeight,
			Inserted.Nutrifact_ID			
		
		FROM 
			Inserted
			INNER JOIN Deleted ON Deleted.Item_Key = Inserted.Item_Key
        
		WHERE 
			ISNULL(Inserted.StoreJurisdictionID, 0)			<> ISNULL(Deleted.StoreJurisdictionID, 0)		OR
			ISNULL(Inserted.Scale_Description1,-1)			<> ISNULL(Deleted.Scale_Description1,-1)        OR
			ISNULL(Inserted.Scale_Description2,-1)			<> ISNULL(Deleted.Scale_Description2,-1)        OR
			ISNULL(Inserted.Scale_Description3,-1)			<> ISNULL(Deleted.Scale_Description3,-1)        OR
			ISNULL(Inserted.Scale_Description4,-1)			<> ISNULL(Deleted.Scale_Description4,-1)        OR
			ISNULL(Inserted.Scale_ExtraText_ID,-1)			<> ISNULL(Deleted.Scale_ExtraText_ID,-1)        OR
			ISNULL(Inserted.Scale_Tare_ID,-1)				<> ISNULL(Deleted.Scale_Tare_ID,-1)             OR
			ISNULL(Inserted.Scale_LabelStyle_ID,-1)			<> ISNULL(Deleted.Scale_LabelStyle_ID,-1)       OR
			ISNULL(Inserted.Scale_ScaleUOMUnit_ID,-1)		<> ISNULL(Deleted.Scale_ScaleUOMUnit_ID,-1)		OR
			ISNULL(Inserted.Scale_RandomWeightType_ID,-1)	<> ISNULL(Deleted.Scale_RandomWeightType_ID,-1) OR
			ISNULL(Inserted.Scale_FixedWeight,'')			<> ISNULL(Deleted.Scale_FixedWeight,'')			OR
			ISNULL(Inserted.Scale_ByCount,-1)				<> ISNULL(Deleted.Scale_ByCount,-1)				OR
			ISNULL(Inserted.ShelfLife_Length,-1)			<> ISNULL(Deleted.ShelfLife_Length,-1)	  		OR
			ISNULL(Inserted.ForceTare,-1)					<> ISNULL(Deleted.ForceTare,-1)					OR
			ISNULL(Inserted.Scale_Alternate_Tare_ID,-1)		<> ISNULL(Deleted.Scale_Alternate_Tare_ID,-1)	OR
			ISNULL(Inserted.Scale_EatBy_ID,-1)				<> ISNULL(Deleted.Scale_EatBy_ID,-1)			OR
			ISNULL(Inserted.Scale_Grade_ID,-1)				<> ISNULL(Deleted.Scale_Grade_ID,-1)			OR
			ISNULL(Inserted.PrintBlankEatBy,-1)				<> ISNULL(Deleted.PrintBlankEatBy,-1)			OR
			ISNULL(Inserted.PrintBlankPackDate,-1)			<> ISNULL(Deleted.PrintBlankPackDate,-1)		OR
			ISNULL(Inserted.PrintBlankShelfLife,-1)			<> ISNULL(Deleted.PrintBlankShelfLife,-1)		OR
			ISNULL(Inserted.PrintBlankTotalPrice,-1)		<> ISNULL(Deleted.PrintBlankTotalPrice,-1)		OR
			ISNULL(Inserted.PrintBlankUnitPrice,-1)			<> ISNULL(Deleted.PrintBlankUnitPrice,-1)		OR
			ISNULL(Inserted.PrintBlankWeight,-1)			<> ISNULL(Deleted.PrintBlankWeight,-1)			OR
			ISNULL(Inserted.Nutrifact_ID,-1)				<> ISNULL(Deleted.Nutrifact_ID,-1)
		
		SELECT @error_no = @@ERROR
    END
   
    IF @error_no = 0
    BEGIN
		-- Create a change entry in the PLUMCorpChgQueue table to send the 
		-- updated corporate scale information from IRMA to the scale systems.
		
		-- Note:  The scale data in IRMA is categorized as “Corporate” (same for all stores in the region) or 
		-- “Zone” (store specific) data.  Only prices are treated as “Zone” data.  This means that adding new 
		-- alternate jurisdiction override values for scale information or changing existing jurisdiction overrides 
		-- will communicate a change in corporate scale data to all stores, even if the jurisdiction data for a 
		-- particular store did not change. 
		INSERT INTO 
			PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		SELECT 
			Inserted.Item_Key, 'C', s.Store_No
		FROM 
			Inserted
			INNER JOIN Deleted ON Deleted.Item_Key = Inserted.Item_Key
			CROSS JOIN Store s
			JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No			
		WHERE
		   (ISNULL(Inserted.Scale_Description1,-1)			<> ISNULL(Deleted.Scale_Description1,-1)        OR
			ISNULL(Inserted.Scale_Description2,-1)			<> ISNULL(Deleted.Scale_Description2,-1)        OR
			ISNULL(Inserted.Scale_Description3,-1)			<> ISNULL(Deleted.Scale_Description3,-1)        OR
			ISNULL(Inserted.Scale_Description4,-1)			<> ISNULL(Deleted.Scale_Description4,-1)        OR
			ISNULL(Inserted.Scale_ExtraText_ID,-1)			<> ISNULL(Deleted.Scale_ExtraText_ID,-1)        OR
			ISNULL(Inserted.Scale_Tare_ID,-1)				<> ISNULL(Deleted.Scale_Tare_ID,-1)             OR
			ISNULL(Inserted.Scale_LabelStyle_ID,-1)			<> ISNULL(Deleted.Scale_LabelStyle_ID,-1)       OR
			ISNULL(Inserted.Scale_ScaleUOMUnit_ID,-1)		<> ISNULL(Deleted.Scale_ScaleUOMUnit_ID,-1)		OR
			ISNULL(Inserted.Scale_RandomWeightType_ID,-1)	<> ISNULL(Deleted.Scale_RandomWeightType_ID,-1) OR
			ISNULL(Inserted.Scale_FixedWeight,'')			<> ISNULL(Deleted.Scale_FixedWeight,'')			OR
			ISNULL(Inserted.Scale_ByCount,-1)				<> ISNULL(Deleted.Scale_ByCount,-1)				OR
			ISNULL(Inserted.ShelfLife_Length,-1)			<> ISNULL(Deleted.ShelfLife_Length,-1)			OR
			ISNULL(Inserted.ForceTare,-1)					<> ISNULL(Deleted.ForceTare,-1)					OR
			ISNULL(Inserted.Scale_Alternate_Tare_ID,-1)		<> ISNULL(Deleted.Scale_Alternate_Tare_ID,-1)	OR
			ISNULL(Inserted.Scale_EatBy_ID,-1)				<> ISNULL(Deleted.Scale_EatBy_ID,-1)			OR
			ISNULL(Inserted.Scale_Grade_ID,-1)				<> ISNULL(Deleted.Scale_Grade_ID,-1)			OR
			ISNULL(Inserted.PrintBlankEatBy,-1)				<> ISNULL(Deleted.PrintBlankEatBy,-1)			OR
			ISNULL(Inserted.PrintBlankPackDate,-1)			<> ISNULL(Deleted.PrintBlankPackDate,-1)		OR
			ISNULL(Inserted.PrintBlankShelfLife,-1)			<> ISNULL(Deleted.PrintBlankShelfLife,-1)		OR
			ISNULL(Inserted.PrintBlankTotalPrice,-1)		<> ISNULL(Deleted.PrintBlankTotalPrice,-1)		OR
			ISNULL(Inserted.PrintBlankUnitPrice,-1)			<> ISNULL(Deleted.PrintBlankUnitPrice,-1)		OR
			ISNULL(Inserted.PrintBlankWeight,-1)			<> ISNULL(Deleted.PrintBlankWeight,-1)			OR
			ISNULL(Inserted.Nutrifact_ID,-1)				<> ISNULL(Deleted.Nutrifact_ID,-1))				AND

			s.WFM_Store = 1 AND si.Authorized = 1 AND
		    NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C') AND
		    NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C') AND
			inserted.StoreJurisdictionID = s.StoreJurisdictionID

		SELECT @Error_No = @@ERROR
	END
	
    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_ItemScaleOverrideUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
CREATE TRIGGER Scale_ItemScaleOverrideAdd ON [dbo].[ItemScaleOverride]
FOR INSERT

AS

-- ****************************************************************************************************************
-- Procedure: Scale_ItemScaleOverrideAdd()
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-25	KM		9382	Add update history template; account for all new ItemScaleOverride values in 4.8;
-- 2013-01-31	KM		9393	Include Nutrifact_ID column;
-- 2013-02-06   MZ      9392    Added a where statement to restrict PLUMCorpChgQueue records to be created only for 
--                              stores in the same jurisdiction.
-- ****************************************************************************************************************

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    IF @error_no = 0
    BEGIN
		-- CREATE THE INITIAL HISTORY RECORD TO TRACK THE CHANGE
		INSERT INTO ItemScaleOverrideHistory (
			Item_Key, 
			StoreJurisdictionID,
			Scale_Description1,
			Scale_Description2,
			Scale_Description3,
			Scale_Description4,
			Scale_ExtraText_ID,
			Scale_Tare_ID,
			Scale_LabelStyle_ID,
			Scale_ScaleUOMUnit_ID,
			Scale_RandomWeightType_ID,
			Scale_FixedWeight,
			Scale_ByCount,
			ShelfLife_Length,
			ForceTare,
			Scale_Alternate_Tare_ID,
			Scale_EatBy_ID,
			Scale_Grade_ID,
			PrintBlankEatBy,
			PrintBlankPackDate,
			PrintBlankShelfLife,
			PrintBlankTotalPrice,
			PrintBlankUnitPrice,
			PrintBlankWeight,
			Nutrifact_ID			
		) SELECT	
			Inserted.Item_Key, 
			Inserted.StoreJurisdictionID,
			Inserted.Scale_Description1,
			Inserted.Scale_Description2,
			Inserted.Scale_Description3,
			Inserted.Scale_Description4,
			Inserted.Scale_ExtraText_ID,
			Inserted.Scale_Tare_ID,
			Inserted.Scale_LabelStyle_ID,
			Inserted.Scale_ScaleUOMUnit_ID,
			Inserted.Scale_RandomWeightType_ID,
			Inserted.Scale_FixedWeight,
			Inserted.Scale_ByCount,
			Inserted.ShelfLife_Length,
			Inserted.ForceTare,
			Inserted.Scale_Alternate_Tare_ID,
			Inserted.Scale_EatBy_ID,
			Inserted.Scale_Grade_ID,
			Inserted.PrintBlankEatBy,
			Inserted.PrintBlankPackDate,
			Inserted.PrintBlankShelfLife,
			Inserted.PrintBlankTotalPrice,
			Inserted.PrintBlankUnitPrice,
			Inserted.PrintBlankWeight,
			Inserted.Nutrifact_ID
		FROM Inserted
            
		SELECT @error_no = @@ERROR
    END
              
    IF @error_no = 0
    BEGIN
		-- Create a change entry in the PLUMCorpChgQueue table to send the 
		-- updated corporate scale information from IRMA to the scale systems.
		
		-- Note:  The scale data in IRMA is categorized as “Corporate” (same for all stores in the region) or 
		-- “Zone” (store specific) data.  Only prices are treated as “Zone” data.  This means that adding new 
		-- alternate jurisdiction override values for scale information or changing existing jurisdiction overrides 
		-- will communicate a change in corporate scale data to all stores, even if the jurisdiction data for a 
		-- particular store did not change. 
		
		-- Matt U. for bug 8709 - I'm updating this to insert an 'A' instead of a 'C' into the Queue. 
		-- When an item gets added to a jurisdiction, it is also getting assigned a retail price,
		-- and as a new retail for an existing item, the maintenance gets handled as a zone-level price change
		-- instead of a corporate item change.
		INSERT INTO 
			PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		SELECT 
			Inserted.Item_Key, 'A', s.Store_No
		FROM 
			Inserted
		CROSS JOIN 
			Store s
		JOIN 
			StoreItem si on si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
		WHERE
	        s.WFM_Store = 1 AND si.Authorized = 1
	        AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A')
			AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE  pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')		
			AND inserted.StoreJurisdictionID = s.StoreJurisdictionID

		SELECT @Error_No = @@ERROR
	END
	
    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_ItemScaleOverrideAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMAReports]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScaleOverride] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScaleOverride] TO [iCONReportingRole]
    AS [dbo];

