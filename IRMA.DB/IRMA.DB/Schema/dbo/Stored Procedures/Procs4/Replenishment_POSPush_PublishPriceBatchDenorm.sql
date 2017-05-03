
CREATE PROCEDURE dbo.Replenishment_POSPush_PublishPriceBatchDenorm

AS
	/* SET NOCOUNT ON */
BEGIN

	DECLARE @RowsToUpdate			INT = 0
	DECLARE @CurrentRow				INT = 0 
	DECLARE @ErrorCode				INT = 0

	DECLARE @PriceBatchDenormID		INT
	DECLARE @InsertDate				DATETIME
	DECLARE @Item_Key				INT
	DECLARE @Identifier				VARCHAR(13)
	DECLARE @POS_Description		VARCHAR(26)
	DECLARE @Item_Description		VARCHAR(60)
	DECLARE @Package_Unit			VARCHAR(5)
	DECLARE @IsScaleItem			TINYINT
	DECLARE @New_Item				TINYINT
	DECLARE @Item_Change			BIT
	DECLARE @Price_Change			BIT
	DECLARE @On_Sale				BIT
	DECLARE @DiscontinueItem		BIT
	DECLARE @Price					MONEY
	DECLARE @Sale_Price				MONEY
	DECLARE @Sale_End_Date			SMALLDATETIME
	DECLARE @Sale_Start_Date		SMALLDATETIME
	DECLARE	@Brand_ID				INT
	DECLARE @Brand_Name				VARCHAR(25)
	DECLARE @CaseSize				DECIMAL(9, 4)
	DECLARE @Item_Size				DECIMAL(9, 4)
	DECLARE @Sign_Description		VARCHAR(60)
	DECLARE @BusinessUnit_ID		INT
	DECLARE @Organic				BIT
	DECLARE @ClassID				INT
	DECLARE	@NatCatID				INT
	DECLARE	@NatFamilyID			INT
	DECLARE @PriceChgTypeDesc		VARCHAR(3)
	DECLARE @ECommerce				BIT
	DECLARE @TaxClassID				INT
	DECLARE @Check_Box_1			BIT
	DECLARE @Check_Box_2			BIT
	DECLARE @Check_Box_3			BIT
	DECLARE @Check_Box_4			BIT
	DECLARE @Check_Box_5			BIT
	DECLARE @Check_Box_6			BIT
	DECLARE @Check_Box_7			BIT
	DECLARE @Check_Box_8			BIT
	DECLARE @Check_Box_9			BIT
	DECLARE @Check_Box_10			BIT
	DECLARE @Check_Box_11			BIT
	DECLARE @Check_Box_12			BIT
	DECLARE @Check_Box_13			BIT
	DECLARE @Check_Box_14			BIT
	DECLARE @Check_Box_15			BIT
	DECLARE @Check_Box_16			BIT
	DECLARE @Check_Box_17			BIT
	DECLARE @Check_Box_18			BIT
	DECLARE @Check_Box_19			BIT
	DECLARE @Check_Box_20			BIT
	DECLARE @Text_1					VARCHAR(50)
	DECLARE @Text_2					VARCHAR(50)
	DECLARE @Text_3					VARCHAR(50)
	DECLARE @Text_4					VARCHAR(50)
	DECLARE @Text_5					VARCHAR(50)
	DECLARE @Text_6					VARCHAR(50)
	DECLARE @Text_7					VARCHAR(50)
	DECLARE @Text_8					VARCHAR(50)
	DECLARE @Text_9					VARCHAR(50)
	DECLARE @Text_10				VARCHAR(50)
	DECLARE @IsDeleted				BIT
	DECLARE @IsAuthorized			BIT
	DECLARE @ADB_SUBJECT			VARCHAR(255)
	DECLARE @ADB_TIMESTAMP			DATETIME
	DECLARE @ADB_OPCODE				INT
	DECLARE @ADB_REF_OBJECT			VARCHAR(64)
	DECLARE @ADB_L_DELIVERY_STATUS	CHAR(1)
	DECLARE @NumOfStores			INT

	DECLARE @ECommerceStores		TABLE
	(BusinessUnit_ID INT NULL)

	DECLARE @P_PriceBatchDenorm_Temp	TABLE 
	(
	[RowID] [int] IDENTITY(1,1) NOT NULL,
	[PriceBatchDenormID] [int] NULL,
	[InsertDate] [datetime] NULL,
	[Item_Key] [int] NULL,
	[Identifier] [varchar](13) NULL,
	[On_Sale] [bit] NULL,
	[POS_Description] [varchar](26) NULL,
	[Item_Description] [varchar](60) NULL,
	[Package_Unit] [varchar](5) NULL,
	[Price_Change] [tinyint] NULL,
	[Item_Change] [tinyint] NULL,
	[IsScaleItem] [tinyint] NULL,
	[Price] [money] NULL,
	[Sale_Price] [money] NULL,
	[Sale_End_Date] [smalldatetime] NULL,
	[Sale_Start_Date] [smalldatetime] NULL,
	[Brand_Name] [varchar](25) NULL,
	[CaseSize] [decimal](9, 4) NULL,
	[Sign_Description] [varchar](60) NULL,
	[BusinessUnit_ID] [int] NULL,
	[Organic] [bit] NULL,
	[ClassID] [int] NULL,
	[PriceChgTypeDesc] [varchar](3) NULL,
	[ECommerce] [bit] NULL,
	[TaxClassID] [int] NULL,
	[DiscontinueItem] [bit] NULL,
	[Check_Box_1] [bit] NULL,
	[Check_Box_2] [bit] NULL,
	[Check_Box_3] [bit] NULL,
	[Check_Box_4] [bit] NULL,
	[Check_Box_5] [bit] NULL,
	[Check_Box_6] [bit] NULL,
	[Check_Box_7] [bit] NULL,
	[Check_Box_8] [bit] NULL,
	[Check_Box_9] [bit] NULL,
	[Check_Box_10] [bit] NULL,
	[Check_Box_11] [bit] NULL,
	[Check_Box_12] [bit] NULL,
	[Check_Box_13] [bit] NULL,
	[Check_Box_14] [bit] NULL,
	[Check_Box_15] [bit] NULL,
	[Check_Box_16] [bit] NULL,
	[Check_Box_17] [bit] NULL,
	[Check_Box_18] [bit] NULL,
	[Check_Box_19] [bit] NULL,
	[Check_Box_20] [bit] NULL,
	[Text_1] [varchar](50) NULL,
	[Text_2] [varchar](50) NULL,
	[Text_3] [varchar](50) NULL,
	[Text_4] [varchar](50) NULL,
	[Text_5] [varchar](50) NULL,
	[Text_6] [varchar](50) NULL,
	[Text_7] [varchar](50) NULL,
	[Text_8] [varchar](50) NULL,
	[Text_9] [varchar](50) NULL,
	[Text_10] [varchar](50) NULL,
	[ADB_SUBJECT] [varchar](255) NULL,
	[ADB_SEQUENCE] [int] NULL,
	[ADB_SET_SEQUENCE] [int] NULL,
	[ADB_TIMESTAMP] [datetime] NULL,
	[ADB_OPCODE] [int] NULL,
	[ADB_UPDATE_ALL] [int] NULL,
	[ADB_REF_OBJECT] [varchar](64) NULL,
	[ADB_L_DELIVERY_STATUS] [char](1) NULL,
	[ADB_L_CMSEQUENCE] [decimal](28, 0) NULL,
	[ADB_TRACKINGID] [varchar](40) NULL,
	[IsDeleted] [bit] NULL,
	[IsAuthorized] [bit] NULL,
	[Item_Size] [decimal](9, 4),
	[NatCatID] [int],
	[NatFamily] [int],
	[Brand_ID] [int],
	[New_Item] [tinyint]
	)


	-- Purging old records in the publishing table
	DELETE FROM P_PriceBatchDenorm
		WHERE (ADB_L_DELIVERY_STATUS = 'C' OR
		       ADB_L_DELIVERY_STATUS = 'F') AND
			  (ADB_TIMESTAMP <= DATEADD(d,-15,GETDATE()))

	INSERT INTO @P_PriceBatchDenorm_Temp
	SELECT *
	FROM P_PriceBatchDenorm pbdp
	WHERE pbdp.ADB_L_DELIVERY_STATUS = 'F'

	SET @RowsToUpdate = @@ROWCOUNT
	
	-- Get a list of stores to be published to the publishing table

	INSERT INTO @ECommerceStores 
		SELECT Key_Value AS BusinessUnit_ID FROM 
		dbo.fn_ParseStringList(
			(SELECT acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = 'POS PUSH JOB' AND
			ack.Name = 'BusinessUnits' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)),'|')
			
	DELETE FROM @ECommerceStores WHERE BusinessUnit_ID IS NULL OR BusinessUnit_ID = 0
	
	SET @NumOfStores = (SELECT COUNT(*) FROM @ECommerceStores)


	BEGIN TRANSACTION PublishDenorm
	
	IF @NumOfStores = 0
		INSERT INTO P_PriceBatchDenorm(
			PriceBatchDenormID,
			InsertDate,
			Item_Key,
			Identifier,
			On_Sale,
			POS_Description,
			Item_Description,
			Package_Unit_Abbr,
			Price_Change,
			Item_Change,
			IsScaleItem,
			Price,
			Sale_Price,
			Sale_End_Date,
			Sale_Start_Date,
			Brand_Name,
			CaseSize,
			Sign_Description,
			BusinessUnit_ID,
			Organic,
			ClassID,
			PriceChgTypeDesc,
			ECommerce,
			TaxClassID,
			DiscontinueItem,
			Check_Box_1,
			Check_Box_2,
			Check_Box_3,
			Check_Box_4,
			Check_Box_5,
			Check_Box_6,
			Check_Box_7,
			Check_Box_8,
			Check_Box_9,
			Check_Box_10,
			Check_Box_11,
			Check_Box_12,
			Check_Box_13,
			Check_Box_14,
			Check_Box_15,
			Check_Box_16,
			Check_Box_17,
			Check_Box_18,
			Check_Box_19,
			Check_Box_20,
			Text_1,
			Text_2,
			Text_3,
			Text_4,
			Text_5,
			Text_6,
			Text_7,
			Text_8,
			Text_9,
			Text_10,
			IsDeleted,
			IsAuthorized,
			Package_Desc2,
			NatCatID,
			NatFamilyID,
			Brand_ID,
			New_Item,
			ADB_OPCODE,
			ADB_L_DELIVERY_STATUS)
		SELECT 
			PriceBatchDenormID, 
			InsertDate, 
			Item_Key, 
			Identifier, 
			On_Sale, 
			POS_Description, 
			Item_Description, 
			Package_Unit_Abbr, 
			Price_Change, 
			Item_Change, 
			IsScaleItem,
			Price, 
			Sale_Price, 
			Sale_End_Date, 
			Sale_Start_Date, 
			Brand_Name, 
			CaseSize, 
			Sign_Description, 
			BusinessUnit_ID, 
			Organic, 
			ClassID, 
			PriceChgTypeDesc, 
			ECommerce, 
			TaxClassID, 
			DiscontinueItem, 
			Check_Box_1, 
			Check_Box_2, 
			Check_Box_3, 
			Check_Box_4, 
			Check_Box_5, 
			Check_Box_6, 
			Check_Box_7, 
			Check_Box_8, 
			Check_Box_9, 
			Check_Box_10, 
			Check_Box_11, 
			Check_Box_12, 
			Check_Box_13, 
			Check_Box_14, 
			Check_Box_15, 
			Check_Box_16, 
			Check_Box_17, 
			Check_Box_18, 
			Check_Box_19, 
			Check_Box_20, 
			Text_1, 
			Text_2, 
			Text_3, 
			Text_4, 
			Text_5, 
			Text_6, 
			Text_7, 
			Text_8, 
			Text_9, 
			Text_10,
			IsDeleted,
			IsAuthorized,
			ISNULL(Package_Desc2, CaseSize),
			NatCatID,
			NatFamilyID,
			Brand_ID,
			ISNULL(New_Item, 0),
			1, 
			'N'
		FROM PriceBatchDenorm pbdt
		WHERE pbdt.PriceBatchDenormID NOT IN
		(SELECT 
		 PriceBatchDenormID
		 FROM @P_PriceBatchDenorm_Temp ppbdt) AND
		 NOT ((ISNULL(pbdt.Text_10,'') = 'DEAUTH') OR
		 (ISNULL(pbdt.Text_10,'') = 'DISCO') OR
		 (ISNULL(pbdt.Text_10,'') = 'ECOM'))
	
	ELSE
	
		INSERT INTO P_PriceBatchDenorm(
			PriceBatchDenormID,
			InsertDate,
			Item_Key,
			Identifier,
			On_Sale,
			POS_Description,
			Item_Description,
			Package_Unit_Abbr,
			Price_Change,
			Item_Change,
			IsScaleItem,
			Price,
			Sale_Price,
			Sale_End_Date,
			Sale_Start_Date,
			Brand_Name,
			CaseSize,
			Sign_Description,
			BusinessUnit_ID,
			Organic,
			ClassID,
			PriceChgTypeDesc,
			ECommerce,
			TaxClassID,
			DiscontinueItem,
			Check_Box_1,
			Check_Box_2,
			Check_Box_3,
			Check_Box_4,
			Check_Box_5,
			Check_Box_6,
			Check_Box_7,
			Check_Box_8,
			Check_Box_9,
			Check_Box_10,
			Check_Box_11,
			Check_Box_12,
			Check_Box_13,
			Check_Box_14,
			Check_Box_15,
			Check_Box_16,
			Check_Box_17,
			Check_Box_18,
			Check_Box_19,
			Check_Box_20,
			Text_1,
			Text_2,
			Text_3,
			Text_4,
			Text_5,
			Text_6,
			Text_7,
			Text_8,
			Text_9,
			Text_10,
			IsDeleted,
			IsAuthorized,
			Package_Desc2,
			NatCatID,
			NatFamilyID,
			Brand_ID,
			New_Item,
			ADB_OPCODE,
			ADB_L_DELIVERY_STATUS)
		SELECT 
			PriceBatchDenormID, 
			InsertDate, 
			Item_Key, 
			Identifier, 
			On_Sale, 
			POS_Description, 
			Item_Description, 
			Package_Unit_Abbr, 
			Price_Change, 
			Item_Change, 
			IsScaleItem,
			Price, 
			Sale_Price, 
			Sale_End_Date, 
			Sale_Start_Date, 
			Brand_Name, 
			CaseSize, 
			Sign_Description, 
			pbdt.BusinessUnit_ID, 
			Organic, 
			ClassID, 
			PriceChgTypeDesc, 
			ECommerce, 
			TaxClassID, 
			DiscontinueItem, 
			Check_Box_1, 
			Check_Box_2, 
			Check_Box_3, 
			Check_Box_4, 
			Check_Box_5, 
			Check_Box_6, 
			Check_Box_7, 
			Check_Box_8, 
			Check_Box_9, 
			Check_Box_10, 
			Check_Box_11, 
			Check_Box_12, 
			Check_Box_13, 
			Check_Box_14, 
			Check_Box_15, 
			Check_Box_16, 
			Check_Box_17, 
			Check_Box_18, 
			Check_Box_19, 
			Check_Box_20, 
			Text_1, 
			Text_2, 
			Text_3, 
			Text_4, 
			Text_5, 
			Text_6, 
			Text_7, 
			Text_8, 
			Text_9, 
			Text_10,
			IsDeleted,
			IsAuthorized,
			ISNULL(Package_Desc2, CaseSize),
			NatCatID,
			NatFamilyID,
			Brand_ID,
			ISNULL(New_Item, 0),
			1, 
			'N'
		FROM PriceBatchDenorm pbdt INNER JOIN @ECommerceStores ecom
		ON pbdt.BusinessUnit_ID = ecom.BusinessUnit_ID
		WHERE pbdt.PriceBatchDenormID NOT IN
		(SELECT 
		 PriceBatchDenormID
		 FROM @P_PriceBatchDenorm_Temp ppbdt) AND
		 NOT ((ISNULL(pbdt.Text_10,'') = 'DEAUTH') OR
		 (ISNULL(pbdt.Text_10,'') = 'DISCO') OR
		 (ISNULL(pbdt.Text_10,'') = 'ECOM'))
		
		 SELECT @ErrorCode = @@ERROR 
		IF (@ErrorCode <> 0) GOTO RollbackTransaction
	
		 /*
		 DO NOT UPDATE / OVERWRITE THE FOLLOWING FIELDS DIRECTLY, USE LOGICAL OR:
		 Item_Change
		 Price_Change
		 On_Sale
		 DiscontinueItem
		*/
		 IF @RowsToUpdate > 0
			BEGIN
				WHILE @CurrentRow < @RowsToUpdate
					BEGIN
						SET @CurrentRow = @CurrentRow + 1
						SELECT 
							@Item_Key = Item_Key,
							@Identifier = Identifier
						FROM @P_PriceBatchDenorm_Temp ppbdt
						WHERE ppbdt.RowID = @CurrentRow
	
						SELECT 
						@Identifier			= pbdt.Identifier,
						@POS_Description	= pbdt.POS_Description,
						@Item_Description	= pbdt.Item_Description,
						@Package_Unit		= pbdt.Package_Unit_Abbr,
						@IsScaleItem		= pbdt.IsScaleItem,
						@Item_Change		= pbdt.Item_Change,
						@Price_Change		= pbdt.Price_Change,
						@On_Sale			= pbdt.On_Sale,
						@New_Item			= pbdt.New_Item,
						@DiscontinueItem	= pbdt.DiscontinueItem,
						@Price				= pbdt.Price,
						@Sale_Price			= pbdt.Sale_Price,
						@Sale_End_Date		= pbdt.Sale_End_Date,
						@Sale_Start_Date	= pbdt.Sale_Start_Date,
						@Brand_Name			= pbdt.Brand_Name,
						@CaseSize			= pbdt.CaseSize,
						@Sign_Description	= pbdt.Sign_Description,
						@BusinessUnit_ID	= pbdt.BusinessUnit_ID,
						@Organic			= pbdt.Organic,
						@ClassID			= pbdt.ClassID,
						@PriceChgTypeDesc	= pbdt.PriceChgTypeDesc,
						@ECommerce			= pbdt.ECommerce,
						@TaxClassID			= pbdt.TaxClassID,
						@Check_Box_1		= pbdt.Check_Box_1,
						@Check_Box_2		= pbdt.Check_Box_2,
						@Check_Box_3		= pbdt.Check_Box_3,
						@Check_Box_4		= pbdt.Check_Box_4,
						@Check_Box_5		= pbdt.Check_Box_5,
						@Check_Box_6		= pbdt.Check_Box_6,
						@Check_Box_7		= pbdt.Check_Box_7,
						@Check_Box_8		= pbdt.Check_Box_8,
						@Check_Box_9		= pbdt.Check_Box_9,
						@Check_Box_10		= pbdt.Check_Box_10,
						@Check_Box_11		= pbdt.Check_Box_11,
						@Check_Box_12		= pbdt.Check_Box_12,
						@Check_Box_13		= pbdt.Check_Box_13,
						@Check_Box_14		= pbdt.Check_Box_14,
						@Check_Box_15		= pbdt.Check_Box_15,
						@Check_Box_16		= pbdt.Check_Box_16,
						@Check_Box_17		= pbdt.Check_Box_17,
						@Check_Box_18		= pbdt.Check_Box_18,
						@Check_Box_19		= pbdt.Check_Box_19,
						@Check_Box_20		= pbdt.Check_Box_20,
						@Text_1				= pbdt.Text_1,
						@Text_2				= pbdt.Text_2,
						@Text_3				= pbdt.Text_3,
						@Text_4				= pbdt.Text_4,
						@Text_5				= pbdt.Text_5,
						@Text_6				= pbdt.Text_6,
						@Text_7				= pbdt.Text_7,
						@Text_8				= pbdt.Text_8,
						@Text_9				= pbdt.Text_9,
						@Text_10			= pbdt.Text_10,
						@IsDeleted			= pbdt.IsDeleted,
						@IsAuthorized		= pbdt.IsAuthorized,
						@Item_Size			= pbdt.Package_Desc2,
						@NatCatID			= pbdt.NatCatID,
						@NatFamilyID		= pbdt.NatFamilyID,
						@Brand_ID			= pbdt.Brand_ID
						FROM PriceBatchDenorm pbdt 
						WHERE pbdt.Item_Key = @Item_Key AND
						pbdt.Identifier = @Identifier
	

						UPDATE P_PriceBatchDenorm
							SET 
						POS_Description = @POS_Description,
						Item_Description = @Item_Description,
						Package_Unit_Abbr = @Package_Unit,
						IsScaleItem = @IsScaleItem,
						New_Item = @New_Item,
						Item_Change = (CASE WHEN Item_Change = 0 THEN @Item_Change END),
						Price_Change = (CASE WHEN Price_Change = 0 THEN @Price_Change END),
						On_Sale = (CASE WHEN On_Sale = 0 THEN @On_Sale END),
						DiscontinueItem = (CASE WHEN DiscontinueItem = 0 THEN @DiscontinueItem END),
						Price = @Price,
						Sale_Price = @Sale_Price,
						Sale_End_Date = @Sale_End_Date,
						Sale_Start_Date = @Sale_Start_Date,
						Brand_Name = @Brand_Name,
						CaseSize = @CaseSize,
						Sign_Description = @Sign_Description,
						BusinessUnit_ID = @BusinessUnit_ID,
						Organic = @Organic,
						ClassID = @ClassID,
						PriceChgTypeDesc = @PriceChgTypeDesc,
						ECommerce = @ECommerce,
						TaxClassID = @TaxClassID,
						Check_Box_1 = @Check_Box_1,
						Check_Box_2 = @Check_Box_2,
						Check_Box_3 = @Check_Box_3,
						Check_Box_4 = @Check_Box_4,
						Check_Box_5 = @Check_Box_5,
						Check_Box_6 = @Check_Box_6,
						Check_Box_7 = @Check_Box_7,
						Check_Box_8 = @Check_Box_8,
						Check_Box_9 = @Check_Box_9,
						Check_Box_10 = @Check_Box_10,
						Check_Box_11 = @Check_Box_11,
						Check_Box_12 = @Check_Box_12,
						Check_Box_13 = @Check_Box_13,
						Check_Box_14 = @Check_Box_14,
						Check_Box_15 = @Check_Box_15,
						Check_Box_16 = @Check_Box_16,
						Check_Box_17 = @Check_Box_17,
						Check_Box_18 = @Check_Box_18,
						Check_Box_19 = @Check_Box_19,
						Check_Box_20 = @Check_Box_20,
						Text_1 = @Text_1,
						Text_2 = @Text_2,
						Text_3 = @Text_3,
						Text_4 = @Text_4,
						Text_5 = @Text_5,
						Text_6 = @Text_6,
						Text_7 = @Text_7,
						Text_8 = @Text_8,
						Text_9 = @Text_9,
						Text_10 = @Text_10,
						IsDeleted = @IsDeleted,
						IsAuthorized = @IsAuthorized,
						Package_Desc2 = @Item_Size,
						NatCatID = @NatCatID,
						NatFamilyID = @NatFamilyID,
						Brand_ID = @Brand_ID,
						ADB_SUBJECT = NULL,
						ADB_TIMESTAMP = GETDATE(),
						ADB_OPCODE = 1,
						ADB_REF_OBJECT = NULL,
						ADB_L_DELIVERY_STATUS = 'N' 
						WHERE Item_Key = @Item_Key AND
						Identifier = @Identifier
	
					END
			END

			SELECT @ErrorCode = @@ERROR 
			IF (@ErrorCode <> 0) GOTO RollbackTransaction

		DELETE FROM PriceBatchDenorm WHERE NOT (ISNULL(Check_Box_20,0) = 1 AND ISNULL(Text_10,'') IN ('DEAUTH', 'DISCO','ECOM'))

		SELECT @ErrorCode = @@ERROR 
		IF (@ErrorCode <> 0) GOTO RollbackTransaction

	COMMIT TRANSACTION PublishDenorm

	RollbackTransaction:
	IF (@ErrorCode <> 0) 
		ROLLBACK TRANSACTION PublishDenorm




END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PublishPriceBatchDenorm] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PublishPriceBatchDenorm] TO [IRSUser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PublishPriceBatchDenorm] TO [IConInterface]
    AS [dbo];

