CREATE TABLE [dbo].[ItemScale] (
    [ItemScale_ID]              INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Item_Key]                  INT          NOT NULL,
    [Nutrifact_ID]              INT          NULL,
    [Scale_ExtraText_ID]        INT          NULL,
    [Scale_Tare_ID]             INT          NULL,
    [Scale_Alternate_Tare_ID]   INT          NULL,
    [Scale_LabelStyle_ID]       INT          NULL,
    [Scale_EatBy_ID]            INT          NULL,
    [Scale_Grade_ID]            INT          NULL,
    [Scale_RandomWeightType_ID] INT          NULL,
    [Scale_ScaleUOMUnit_ID]     INT          NULL,
    [Scale_FixedWeight]         VARCHAR (25) NULL,
    [Scale_ByCount]             INT          NULL,
    [ForceTare]                 BIT          CONSTRAINT [DF_ItemScale_ForceTare] DEFAULT ((0)) NOT NULL,
    [PrintBlankShelfLife]       BIT          CONSTRAINT [DF_ItemScale_PrintBlankShelfLife] DEFAULT ((0)) NOT NULL,
    [PrintBlankEatBy]           BIT          CONSTRAINT [DF_ItemScale_PrintBlankEatBy] DEFAULT ((0)) NOT NULL,
    [PrintBlankPackDate]        BIT          CONSTRAINT [DF_ItemScale_PrintBlankPackDate] DEFAULT ((0)) NOT NULL,
    [PrintBlankWeight]          BIT          CONSTRAINT [DF_ItemScale_PrintBlankWeight] DEFAULT ((0)) NOT NULL,
    [PrintBlankUnitPrice]       BIT          CONSTRAINT [DF_ItemScale_PrintBlankUnitPrice] DEFAULT ((0)) NOT NULL,
    [PrintBlankTotalPrice]      BIT          CONSTRAINT [DF_ItemScale_PrintBlankTotalPrice] DEFAULT ((0)) NOT NULL,
    [Scale_Description1]        VARCHAR (64) NULL,
    [Scale_Description2]        VARCHAR (64) NULL,
    [Scale_Description3]        VARCHAR (64) NULL,
    [Scale_Description4]        VARCHAR (64) NULL,
    [ShelfLife_Length]          SMALLINT     NULL,
    [Scale_Ingredient_ID]       INT          NULL,
    [Scale_Allergen_ID]         INT          NULL,
    [Scale_StorageData_ID]	    INT          NULL,
    CONSTRAINT [PK_ItemScale] PRIMARY KEY CLUSTERED ([ItemScale_ID] ASC),
    CONSTRAINT [FK_ItemScale_ExtraText1] FOREIGN KEY ([Scale_ExtraText_ID]) REFERENCES [dbo].[Scale_ExtraText] ([Scale_ExtraText_ID]),
    CONSTRAINT [FK_ItemScale_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemScale_ItemUnit] FOREIGN KEY ([Scale_ScaleUOMUnit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemScale_NutriFacts1] FOREIGN KEY ([Nutrifact_ID]) REFERENCES [dbo].[NutriFacts] ([NutriFactsID]) ON DELETE SET NULL,
    CONSTRAINT [FK_ItemScale_Scale_EatBy1] FOREIGN KEY ([Scale_EatBy_ID]) REFERENCES [dbo].[Scale_EatBy] ([Scale_EatBy_ID]),
    CONSTRAINT [FK_ItemScale_Scale_Grade1] FOREIGN KEY ([Scale_Grade_ID]) REFERENCES [dbo].[Scale_Grade] ([Scale_Grade_ID]),
    CONSTRAINT [FK_ItemScale_Scale_LabelStyle1] FOREIGN KEY ([Scale_LabelStyle_ID]) REFERENCES [dbo].[Scale_LabelStyle] ([Scale_LabelStyle_ID]),
    CONSTRAINT [FK_ItemScale_Scale_RandomWeightType1] FOREIGN KEY ([Scale_RandomWeightType_ID]) REFERENCES [dbo].[Scale_RandomWeightType] ([Scale_RandomWeightType_ID]),
    CONSTRAINT [FK_ItemScale_Scale_Tare2] FOREIGN KEY ([Scale_Tare_ID]) REFERENCES [dbo].[Scale_Tare] ([Scale_Tare_ID]),
    CONSTRAINT [FK_ItemScale_Scale_Tare3] FOREIGN KEY ([Scale_Alternate_Tare_ID]) REFERENCES [dbo].[Scale_Tare] ([Scale_Tare_ID]),
    CONSTRAINT [FK_ItemScale_ScaleAllergen] FOREIGN KEY ([Scale_Allergen_ID]) REFERENCES [dbo].[Scale_Allergen] ([Scale_Allergen_ID]),
    CONSTRAINT [FK_ItemScale_ScaleIngredient] FOREIGN KEY ([Scale_Ingredient_ID]) REFERENCES [dbo].[Scale_Ingredient] ([Scale_Ingredient_ID]),
	CONSTRAINT [FK_ItemScale_StorageData] FOREIGN KEY ([Scale_StorageData_ID]) REFERENCES [dbo].[Scale_StorageData] ([Scale_StorageData_ID])
);


GO
ALTER TABLE [dbo].[ItemScale] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_ItemScale_Item_Key]
    ON [dbo].[ItemScale]([Item_Key] ASC)
    INCLUDE([Nutrifact_ID], [Scale_ExtraText_ID], [Scale_Tare_ID], [Scale_Alternate_Tare_ID], [Scale_LabelStyle_ID], [Scale_EatBy_ID], [Scale_Grade_ID], [Scale_ScaleUOMUnit_ID], [Scale_FixedWeight], [Scale_ByCount], [ForceTare], [Scale_Description1], [Scale_Description2], [Scale_Description3], [Scale_Description4], [ShelfLife_Length], [Scale_Ingredient_ID], [Scale_Allergen_ID]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_ItemScale_Nutrifact_ID]
    ON [dbo].[ItemScale]([Nutrifact_ID] ASC)
    INCLUDE([Item_Key], [Scale_LabelStyle_ID]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_ItemScale_Scale_ExtraText_ID]
    ON [dbo].[ItemScale]([Scale_ExtraText_ID] ASC)
    INCLUDE([Item_Key], [Scale_Ingredient_ID], [Scale_Allergen_ID]) WITH (FILLFACTOR = 80);


GO

CREATE Trigger [dbo].[Scale_ItemScale_Add] 
ON [dbo].[ItemScale]
FOR INSERT
AS
BEGIN
	DECLARE @Error_No int = 0

	--PBI 27221: As IRMA I need to stop queuing non-batchable scale maintenance for stores that don't have scale writers/ftp configs set up so that the table does not keep growing.
	DECLARE @store TABLE(Store_No INT, WFM_Store BIT, Mega_Store BIT);
	INSERT INTO @store(Store_No, WFM_Store, Mega_Store)
	SELECT DISTINCT s.Store_No, s.WFM_Store, s.Mega_Store
	FROM Store s
	JOIN StoreFTPConfig sc ON sc.Store_No = s.Store_No
	WHERE sc.FileWriterType = 'SCALE'
	  AND Len(LTrim(RTrim(IsNull(sc.IP_Address, '')))) > 6
	  AND Len(LTrim(RTrim(IsNull(sc.FTP_User, '')))) > 0
	  AND Len(LTrim(RTrim(IsNull(sc.FTP_Password, '')))) > 0;

	IF(Exists(SELECT 1 FROM @store))
	BEGIN
		-- 365 non-scale PLU maintenance is not handled here.  Check GenerateCustomerFacingScaleMaintenance.
		DECLARE @Is365NonScalePlu bit = case when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key) then 1 else 0 end
		DECLARE @Is365CFSPlu bit = case when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key and icfs.SendToScale = 1) then 1 else 0 end

		INSERT INTO
			PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		SELECT 
			Inserted.Item_Key, 'A', s.Store_No
		FROM 
			Inserted
		CROSS JOIN 
			@store s
		JOIN 
			StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
		WHERE 
			(@Is365NonScalePlu = 0 AND (s.WFM_Store = 1 OR s.Mega_Store = 1))
			AND si.Authorized = 1 
			AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') 
			AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')
		UNION
		SELECT 
			Inserted.Item_Key, 'A', s.Store_No
		FROM 
			Inserted
		CROSS JOIN
		@store s
		JOIN
			StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
		WHERE
			(@Is365CFSPlu = 1 AND s.Mega_Store = 1)
			AND si.Authorized = 1 
			AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') 
			AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')

		SELECT @Error_No = @@ERROR

		IF @Error_No <> 0
		BEGIN
			ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			RAISERROR ('Scale_ItemScale_Add trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
	END
END
GO

CREATE Trigger [dbo].[Scale_ItemScale_Update] 
ON [dbo].[ItemScale]
FOR UPDATE
AS
BEGIN
	DECLARE @Error_No int = 0

	--PBI 27221: As IRMA I need to stop queuing non-batchable scale maintenance for stores that don't have scale writers/ftp configs set up so that the table does not keep growing.
	DECLARE @store TABLE(Store_No INT, WFM_Store BIT, Mega_Store BIT);
	INSERT INTO @store(Store_No, WFM_Store, Mega_Store)
	SELECT DISTINCT s.Store_No, s.WFM_Store, s.Mega_Store
	FROM Store s
	JOIN StoreFTPConfig sc ON sc.Store_No = s.Store_No
	WHERE sc.FileWriterType = 'SCALE'
	  AND Len(LTrim(RTrim(IsNull(sc.IP_Address, '')))) > 6
	  AND Len(LTrim(RTrim(IsNull(sc.FTP_User, '')))) > 0
	  AND Len(LTrim(RTrim(IsNull(sc.FTP_Password, '')))) > 0;

	IF(Exists(SELECT 1 FROM @store))
	BEGIN
		DECLARE @UseSmartX INT = (SELECT dbo.fn_InstanceDataValue('UseSmartXPriceData', NULL))
		-- 365 non-scale PLU maintenance is not handled here.  Check GenerateCustomerFacingScaleMaintenance.
		DECLARE @Is365NonScalePlu bit = case when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key) then 1 else 0 end
		DECLARE @Is365CFSPlu bit = case when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key and icfs.SendToScale = 1) then 1 else 0 end
    	
		IF @UseSmartX = 0
			-- For regions not using SmartX scale writer, all scale item changes
			-- should be captured.
			BEGIN
				INSERT INTO 
					PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
				SELECT
					Inserted.Item_Key, 'C', s.Store_No
				FROM 
				Inserted
				CROSS JOIN 
					@store s
				JOIN 
					StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
				WHERE
				((@Is365NonScalePlu = 0 AND (s.WFM_Store = 1 OR s.Mega_Store = 1))
					OR (@Is365CFSPlu = 1 AND (s.Mega_Store = 1)))
					AND si.Authorized = 1 
					AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
					AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
			END
		ELSE
			BEGIN
				INSERT INTO 
					PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
				SELECT 
					Inserted.Item_Key, 'C', s.Store_No
				FROM 
					Inserted
				INNER JOIN 
					Deleted ON Deleted.Item_Key = Inserted.Item_Key
				CROSS JOIN 
					@store s
				JOIN 
					StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
				WHERE
					(ISNULL(Inserted.Scale_Tare_ID,-1)             <> ISNULL(Deleted.Scale_Tare_ID,-1)             OR
					ISNULL(Inserted.Scale_Alternate_Tare_ID,-1)   <> ISNULL(Deleted.Scale_Alternate_Tare_ID,-1)   OR
					ISNULL(Inserted.Scale_LabelStyle_ID,-1)       <> ISNULL(Deleted.Scale_LabelStyle_ID,-1)       OR
					ISNULL(Inserted.Scale_EatBy_ID,-1)            <> ISNULL(Deleted.Scale_EatBy_ID,-1)            OR
					ISNULL(Inserted.Scale_Grade_ID,-1)            <> ISNULL(Deleted.Scale_Grade_ID,-1)            OR
					ISNULL(Inserted.Scale_RandomWeightType_ID,-1) <> ISNULL(Deleted.Scale_RandomWeightType_ID,-1) OR
					ISNULL(Inserted.Scale_ScaleUOMUnit_ID,-1)     <> ISNULL(Deleted.Scale_ScaleUOMUnit_ID,-1)     OR
					ISNULL(Inserted.Scale_FixedWeight,-1)         <> ISNULL(Deleted.Scale_FixedWeight,-1)         OR
					ISNULL(Inserted.Scale_ByCount,-1)             <> ISNULL(Deleted.Scale_ByCount,-1)             OR
					ISNULL(Inserted.Scale_ExtraText_ID,-1)        <> ISNULL(Deleted.Scale_ExtraText_ID,-1)        OR
					ISNULL(Inserted.Nutrifact_ID,-1)              <> ISNULL(Deleted.Nutrifact_ID,-1)              OR
					ISNULL(Inserted.Scale_Ingredient_ID,-1)       <> ISNULL(Deleted.Scale_Ingredient_ID,-1)       OR
					ISNULL(Inserted.Scale_Allergen_ID,-1)         <> ISNULL(Deleted.Scale_Allergen_ID,-1)         OR
					ISNULL(Inserted.ForceTare,-1)                 <> ISNULL(Deleted.ForceTare,-1)                 OR
					ISNULL(Inserted.PrintBlankShelfLife,-1)       <> ISNULL(Deleted.PrintBlankShelfLife,-1)       OR
					ISNULL(Inserted.PrintBlankEatBy,-1)           <> ISNULL(Deleted.PrintBlankEatBy,-1)           OR
					ISNULL(Inserted.PrintBlankPackDate,-1)        <> ISNULL(Deleted.PrintBlankPackDate,-1)        OR
					ISNULL(Inserted.PrintBlankWeight,-1)          <> ISNULL(Deleted.PrintBlankWeight,-1)          OR
					ISNULL(Inserted.PrintBlankUnitPrice,-1)       <> ISNULL(Deleted.PrintBlankUnitPrice,-1)       OR
					ISNULL(Inserted.PrintBlankTotalPrice,-1)      <> ISNULL(Deleted.PrintBlankTotalPrice,-1)      OR
					ISNULL(Inserted.Scale_Description1,-1)        <> ISNULL(Deleted.Scale_Description1,-1)        OR
					ISNULL(Inserted.Scale_Description2,-1)        <> ISNULL(Deleted.Scale_Description2,-1)        OR
					ISNULL(Inserted.Scale_Description3,-1)        <> ISNULL(Deleted.Scale_Description3,-1)        OR
					ISNULL(Inserted.Scale_Description4,-1)        <> ISNULL(Deleted.Scale_Description4,-1)        OR
					ISNULL(Inserted.ShelfLife_Length,-1)          <> ISNULL(Deleted.ShelfLife_Length,-1))        AND

					(@Is365NonScalePlu = 0 AND (s.WFM_Store = 1 OR s.Mega_Store = 1))
					AND si.Authorized = 1
					AND NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode like '[AC]') 
					AND NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode like '[AC]')
			END
		
		SELECT @Error_No = @@ERROR

		IF @Error_No <> 0
			BEGIN
				ROLLBACK TRAN
				DECLARE @Severity smallint
				SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
				RAISERROR ('Scale_ItemScale_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
			END
	END
END
GO

GRANT SELECT
    ON OBJECT::[dbo].[ItemScale] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScale] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScale] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScale] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScale] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScale] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScale] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScale] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScale] TO [IRMAReports]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemScale] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemScale] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScale] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemScale] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScale] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemScale] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScale] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemScale] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScale] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScale] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemScale] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemScale] TO [iCONReportingRole]
    AS [dbo];