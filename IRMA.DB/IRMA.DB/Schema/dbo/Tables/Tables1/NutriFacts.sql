CREATE TABLE [dbo].[NutriFacts] (
    [NutriFactsID]             INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Scale_LabelFormat_ID]     INT             NOT NULL,
    [ServingUnits]             TINYINT         CONSTRAINT [DF_NutriFacts_ServingUnits] DEFAULT ((0)) NULL,
    [Description]              VARCHAR (28)    NULL,
    [ServingsPerPortion]       INT             CONSTRAINT [DF_NutriFacts_Size] DEFAULT ((0)) NULL,
    [SizeWeight]               INT             CONSTRAINT [DF_NutriFacts_SizeWeight] DEFAULT ((0)) NULL,
    [Calories]                 INT             CONSTRAINT [DF_NutriFacts_Calories] DEFAULT ((0)) NULL,
    [CaloriesFat]              INT             CONSTRAINT [DF_NutriFacts_CaloriesFat] DEFAULT ((0)) NULL,
    [CaloriesSaturatedFat]     INT             CONSTRAINT [DF_NutriFacts_CaloriesSaturatedFat] DEFAULT ((0)) NULL,
    [ServingPerContainer]      VARCHAR (10)    CONSTRAINT [DF_NutriFacts_PerContainer] DEFAULT ((0)) NULL,
    [TotalFatWeight]           DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_TotalFatWeight] DEFAULT ((0.0)) NULL,
    [TotalFatPercentage]       SMALLINT        CONSTRAINT [DF_NutriFacts_TotalFatPercentage] DEFAULT ((0)) NULL,
    [SaturatedFatWeight]       DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_SaturatedFatWeight] DEFAULT ((0.0)) NULL,
    [SaturatedFatPercent]      SMALLINT        CONSTRAINT [DF_NutriFacts_SaturatedFatPercent] DEFAULT ((0)) NULL,
    [PolyunsaturatedFat]       DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_PolyunsaturatedFat] DEFAULT ((0.0)) NULL,
    [MonounsaturatedFat]       DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_MonounsaturatedFat] DEFAULT ((0.0)) NULL,
    [CholesterolWeight]        DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_CholesterolWeight] DEFAULT ((0.0)) NULL,
    [CholesterolPercent]       SMALLINT        CONSTRAINT [DF_NutriFacts_CholesterolPercent] DEFAULT ((0)) NULL,
    [SodiumWeight]             DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_SodiumWeight] DEFAULT ((0.0)) NULL,
    [SodiumPercent]            SMALLINT        CONSTRAINT [DF_NutriFacts_SodiumPercent] DEFAULT ((0)) NULL,
    [PotassiumWeight]          DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_PotassiumWeight] DEFAULT ((0.0)) NULL,
    [PotassiumPercent]         SMALLINT        CONSTRAINT [DF_NutriFacts_PotassiumPercent] DEFAULT ((0)) NULL,
    [TotalCarbohydrateWeight]  DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_TotalCarbohydrateWeight] DEFAULT ((0.0)) NULL,
    [TotalCarbohydratePercent] SMALLINT        CONSTRAINT [DF_NutriFacts_TotalCarbohydratePercent] DEFAULT ((0)) NULL,
    [DietaryFiberWeight]       DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_DietaryFiberWeight] DEFAULT ((0.0)) NULL,
    [DietaryFiberPercent]      SMALLINT        CONSTRAINT [DF_NutriFacts_DietaryFiberPercent] DEFAULT ((0)) NULL,
    [SolubleFiber]             DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_SolubleFiber] DEFAULT ((0.0)) NULL,
    [InsolubleFiber]           DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_InsolubleFiber] DEFAULT ((0.0)) NULL,
    [Sugar]                    DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_Sugar] DEFAULT ((0.0)) NULL,
    [SugarAlcohol]             DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_SugarAlcohol] DEFAULT ((0.0)) NULL,
    [OtherCarbohydrates]       DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_OtherCarbohydrates] DEFAULT ((0.0)) NULL,
    [ProteinWeight]            DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_ProteinWeight] DEFAULT ((0.0)) NULL,
    [ProteinPercent]           SMALLINT        CONSTRAINT [DF_NutriFacts_ProteinPercent] DEFAULT ((0)) NULL,
    [VitaminA]                 SMALLINT        CONSTRAINT [DF_NutriFacts_VitaminA] DEFAULT ((0)) NULL,
    [Betacarotene]             SMALLINT        CONSTRAINT [DF_NutriFacts_Betacarotene] DEFAULT ((0)) NULL,
    [VitaminC]                 SMALLINT        CONSTRAINT [DF_NutriFacts_VitaminC] DEFAULT ((0)) NULL,
    [Calcium]                  SMALLINT        CONSTRAINT [DF_NutriFacts_Calcium] DEFAULT ((0)) NULL,
    [Iron]                     SMALLINT        CONSTRAINT [DF_NutriFacts_Iron] DEFAULT ((0)) NULL,
    [VitaminD]                 SMALLINT        CONSTRAINT [DF_NutriFacts_VitaminD] DEFAULT ((0)) NULL,
    [VitaminE]                 SMALLINT        CONSTRAINT [DF_NutriFacts_VitaminE] DEFAULT ((0)) NULL,
    [Thiamin]                  SMALLINT        CONSTRAINT [DF_NutriFacts_Thiamin] DEFAULT ((0)) NULL,
    [Riboflavin]               SMALLINT        CONSTRAINT [DF_NutriFacts_Riboflavin] DEFAULT ((0)) NULL,
    [Niacin]                   SMALLINT        CONSTRAINT [DF_NutriFacts_Niacin] DEFAULT ((0)) NULL,
    [VitaminB6]                SMALLINT        CONSTRAINT [DF_NutriFacts_VitaminB6] DEFAULT ((0)) NULL,
    [Folate]                   SMALLINT        CONSTRAINT [DF_NutriFacts_Folate] DEFAULT ((0)) NULL,
    [VitaminB12]               SMALLINT        CONSTRAINT [DF_NutriFacts_VitaminB12] DEFAULT ((0)) NULL,
    [Biotin]                   SMALLINT        CONSTRAINT [DF_NutriFacts_Biotin] DEFAULT ((0)) NULL,
    [PantothenicAcid]          SMALLINT        CONSTRAINT [DF_NutriFacts_PantothenicAcid] DEFAULT ((0)) NULL,
    [Phosphorous]              SMALLINT        CONSTRAINT [DF_NutriFacts_Phosphorous] DEFAULT ((0)) NULL,
    [Iodine]                   SMALLINT        CONSTRAINT [DF_NutriFacts_Iodine] DEFAULT ((0)) NULL,
    [Magnesium]                SMALLINT        CONSTRAINT [DF_NutriFacts_Magnesium] DEFAULT ((0)) NULL,
    [Zinc]                     SMALLINT        CONSTRAINT [DF_NutriFacts_Zinc] DEFAULT ((0)) NULL,
    [Copper]                   SMALLINT        CONSTRAINT [DF_NutriFacts_Copper] DEFAULT ((0)) NULL,
    [Transfat]                 DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_Transfat] DEFAULT ((0.0)) NULL,
    [CaloriesFromTransFat]     INT             CONSTRAINT [DF_NutriFacts_CaloriesFromTransFat] DEFAULT ((0)) NULL,
    [Om6Fatty]                 DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_Om6Fatty] DEFAULT ((0.0)) NULL,
    [Om3Fatty]                 DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_Om3Fatty] DEFAULT ((0.0)) NULL,
    [Starch]                   DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_Starch] DEFAULT ((0.0)) NULL,
    [Chloride]                 SMALLINT        CONSTRAINT [DF_NutriFacts_Chloride] DEFAULT ((0)) NULL,
    [Chromium]                 SMALLINT        CONSTRAINT [DF_NutriFacts_Chromium] DEFAULT ((0)) NULL,
    [VitaminK]                 SMALLINT        CONSTRAINT [DF_NutriFacts_VitaminK] DEFAULT ((0)) NULL,
    [Manganese]                SMALLINT        CONSTRAINT [DF_NutriFacts_Manganese] DEFAULT ((0)) NULL,
    [Molybdenum]               SMALLINT        CONSTRAINT [DF_NutriFacts_Molybdenum] DEFAULT ((0)) NULL,
    [Selenium]                 SMALLINT        CONSTRAINT [DF_NutriFacts_Selenium] DEFAULT ((0)) NULL,
    [ServingSizeDesc]          VARCHAR (28)    CONSTRAINT [DF_NutriFacts_SizeText] DEFAULT ('') NULL,
    [TransfatWeight]           DECIMAL (10, 1) CONSTRAINT [DF_NutriFacts_TransfatWeight] DEFAULT ((0.0)) NULL,
    [HshRating]                INT             NULL,
    [OverwriteServingSizeFlag] TINYINT         NULL,
    [Descriptors]              NVARCHAR (50)   NULL,
    CONSTRAINT [PK_NutriFacts] PRIMARY KEY CLUSTERED ([NutriFactsID] ASC),
    CONSTRAINT [FK_NutriFacts_Scale_LabelFormat] FOREIGN KEY ([Scale_LabelFormat_ID]) REFERENCES [dbo].[Scale_LabelFormat] ([Scale_LabelFormat_ID])
);


GO
ALTER TABLE [dbo].[NutriFacts] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE Trigger Scale_Nutrifact_Add 
ON [dbo].[NutriFacts]
FOR INSERT
AS
-- **************************************************************************
-- Trigger: Scale_Nutrifact_Add()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This trigger is called to put nutrifact records in the queue to be sent to the scales.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 06/05/2012	BJL   			6583	Added check against the NutriFactsChgQueue and
--										NutriFactsChgQueueTmp tables to prevent duplicate entry.
-- **************************************************************************
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	-- DETERMINE IF CURRENT REGION PUSHES SCALE NUTRIFACT DATA --
    DECLARE @PushScaleNutrifactData bit
    SELECT @PushScaleNutrifactData = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'PushScaleNutrifactData'
           
    IF @Error_No = 0 AND @PushScaleNutrifactData = 1
	BEGIN
		-- Queue for NutriFactsChg
		INSERT INTO 
			NutriFactsChgQueue (NutriFactsID, ActionCode)
		SELECT 
			Inserted.NutriFactsID, 'A'
		FROM 
			Inserted
		WHERE
			NOT EXISTS (SELECT 1 FROM NutriFactsChgQueue NQ WHERE NQ.NutriFactsID = Inserted.NutriFactsID)
		AND NOT EXISTS (SELECT 1 FROM NutriFactsChgQueueTmp NQT WHERE NQT.NutriFact_ID = Inserted.NutriFactsID)

		SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_Nutrifact_Add trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

CREATE Trigger Scale_NutriFact_Update
ON [dbo].[NutriFacts]
FOR UPDATE
AS
-- **************************************************************************
-- Trigger: Scale_NutriFact_Update()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This trigger is called to put nutrifact records in the queue to be sent to the scales.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 06/05/2012	BJL   			6583	Added check against the NutriFactsChgQueue and
--										NutriFactsChgQueueTmp tables to prevent duplicate entry.
-- 07/13/2016   MZ       20166(15920)   Added queuing for PlumCorpChgQueue
-- **************************************************************************
BEGIN
    DECLARE @Error_No int 
    SELECT @Error_No = 0

	-- DETERMINE IF CURRENT REGION PUSHES SCALE NUTRIFACT DATA --
    DECLARE @PushScaleNutrifactData bit
    SELECT @PushScaleNutrifactData = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'PushScaleNutrifactData'

    IF @Error_No = 0 AND @PushScaleNutrifactData = 1
	BEGIN
		INSERT INTO 
			NutriFactsChgQueue (NutriFactsID, ActionCode)
		SELECT 
			Inserted.NutriFactsID, 'C'
		FROM 
			Inserted
		WHERE
			NOT EXISTS (SELECT 1 FROM NutriFactsChgQueue NQ WHERE NQ.NutriFactsID = Inserted.NutriFactsID)
		AND NOT EXISTS (SELECT 1 FROM NutriFactsChgQueueTmp NQT WHERE NQT.NutriFact_ID = Inserted.NutriFactsID)

		SELECT @Error_No = @@ERROR
	END

	-- Queue for PlumCorpChgQueue
	IF @Error_No = 0
	BEGIN
		INSERT INTO 
		        PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
            SELECT 
		        isc.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
            INNER JOIN 
				Deleted ON Deleted.NutriFactsID = Inserted.NutriFactsID
			INNER JOIN ItemScale isc ON isc.Nutrifact_ID = Inserted.NutriFactsID
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
				Deleted ON Deleted.NutriFactsID = Inserted.NutriFactsID
			INNER JOIN ItemScale isc ON isc.Nutrifact_ID = Inserted.NutriFactsID
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
        RAISERROR ('Scale_NutriFact_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
GRANT SELECT
    ON OBJECT::[dbo].[NutriFacts] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NutriFacts] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NutriFacts] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NutriFacts] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NutriFacts] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[NutriFacts] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NutriFacts] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NutriFacts] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NutriFacts] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NutriFacts] TO [IRMAReports]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NutriFacts] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[NutriFacts] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NutriFacts] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[NutriFacts] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NutriFacts] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[NutriFacts] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NutriFacts] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[NutriFacts] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[NutriFacts] TO [iCONReportingRole]
    AS [dbo];

