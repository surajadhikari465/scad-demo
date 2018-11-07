IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_POSPush_PopulatePriceBatchDenorm]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
begin
	EXEC ('CREATE PROCEDURE [dbo].[Replenishment_POSPush_PopulatePriceBatchDenorm] AS SELECT 1')
end
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_PopulatePriceBatchDenorm]    Script Date: 09/19/2013 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE dbo.Replenishment_POSPush_PopulatePriceBatchDenorm
	(
	@PriceBatchDetailData	dbo.PriceBatchDetailType READONLY
	)
AS
--**************************************************************************************
-- Procedure: Replenishment_POSPush_PopulatePriceBatchDenorm
--	  Author: n/a
--      Date: n/a
--
-- Description: This stored proc is called by the TagWriterDAO.vb in IRMA Client
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 
-- 2013-12-20	DN		14677	Updated the column IsScaleItem to use the value for 
--								Sold_By_Weight (for batches). For non-batchable items, 
--								this CASE...WHEN statement is used.
--								CASE WHEN 
--									ISNULL(ISNULL(PU_Override.Weight_Unit, PU.Weight_Unit),0) = 1 AND 
--									dbo.fn_IsScaleItem(II.Identifier) = 0 THEN 1 
--								ELSE 0 END
-- 2014-01-10	DN		14690	Included @ECommerceStores to reduce the size of the returning results.
--**************************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	 --select * from @PriceBatchDetailData
	/* SET NOCOUNT ON */
	-- SELECT COUNT(*) FROM @PriceBatchDetailData -- Use this query to verify Rows in @PriceBatchDetailData = Rows in Return Results.

	--
	IF OBJECT_ID('tempdb..#TempDenorm') IS NOT NULL
	BEGIN
		DROP TABLE #TempDenorm
	END 
	
	IF OBJECT_ID('tempdb..#ECommerceStores') IS NOT NULL
	BEGIN
		DROP TABLE #ECommerceStores
	END 
		
	CREATE TABLE #TempDenorm 
	(Item_Key INT
	, Store_No INT
	)

	DECLARE @Date DATETIME

	-- Check the Store Jurisdiction Flag
	DECLARE @UseStoreJurisdictions int
	SELECT @UseStoreJurisdictions = FlagValue FROM dbo.InstanceDataFlags  WHERE FlagKey = 'UseStoreJurisdictions'

	--Using the regional scale file?
	DECLARE @UseRegionalScaleFile bit
	SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM dbo.InstanceDataFlags  WHERE FlagKey='UseRegionalScaleFile')

	SELECT @Date = CONVERT(DATE,GETDATE())

	CREATE TABLE #ECommerceStores
	(BusinessUnit_ID INT NULL)

	-- Get a list of stores (business units)
	INSERT INTO #ECommerceStores 
		SELECT Key_Value AS BusinessUnit_ID FROM 
		dbo.fn_ParseStringList(
			(SELECT acv.Value
				FROM AppConfigValue acv  
				INNER JOIN AppConfigEnv ace  ON acv.EnvironmentID = ace.EnvironmentID 
				INNER JOIN AppConfigApp aca  ON acv.ApplicationID = aca.ApplicationID 
				INNER JOIN AppConfigKey ack  ON acv.KeyID = ack.KeyID 
				WHERE aca.Name = 'POS PUSH JOB' 
					AND ack.Name = 'BusinessUnits' 
					and SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)),'|')

	BEGIN TRANSACTION PopulateDenorm
	BEGIN TRY
		INSERT INTO #TempDenorm (
			Item_Key,
			Store_No)
			SELECT Item_Key, Store_No
			FROM PriceBatchDenorm  
			WHERE UPPER(Text_10) IN ('DEAUTH', 'DISCO', 'ECOM') 
			AND Check_Box_20 = 1 

		UPDATE PriceBatchDenorm
		SET Check_Box_20 = 0 
		WHERE Item_Key IN 
			(SELECT Item_Key FROM #TempDenorm)
		
		INSERT INTO PriceBatchDenorm
		SELECT 
		GETDATE() AS InsertDate,
		PBDD.*, 
		-- Below are the additional fields requested.
		S.BusinessUnit_ID,
		I.Organic,
		I.ClassID,
		PBD.PriceChgTypeDesc,
		SI.ECommerce AS ECommerce,
		I.TaxClassID,
		SIV.DiscontinueItem,
		IA.Check_Box_1,
		IA.Check_Box_2,
		IA.Check_Box_3,
		IA.Check_Box_4,
		IA.Check_Box_5,
		IA.Check_Box_6,
		IA.Check_Box_7,
		IA.Check_Box_8,
		IA.Check_Box_9,
		IA.Check_Box_10,
		IA.Check_Box_11,
		IA.Check_Box_12,
		IA.Check_Box_13,
		IA.Check_Box_14,
		IA.Check_Box_15,
		IA.Check_Box_16,
		IA.Check_Box_17,
		IA.Check_Box_18,
		IA.Check_Box_19,
		IA.Check_Box_20,
		IA.Text_1,
		IA.Text_2,
		IA.Text_3,
		IA.Text_4,
		IA.Text_5,
		IA.Text_6,
		IA.Text_7,
		IA.Text_8,
		IA.Text_9,
		IA.Text_10,
		I.Remove_Item AS IsDeleted,
		1 AS IsAuthorized,
		NIC.NatCatID,
		NIF.NatFamilyID,
		I.Brand_ID
		FROM @PriceBatchDetailData PBDD
		INNER JOIN Item I   ON	PBDD.Item_Key = I.Item_Key
		INNER JOIN NatItemClass NIS  ON I.ClassID = NIS.ClassID
		INNER JOIN NatItemCat NIC  ON NIS.NatCatID = NIC.NatCatID
		INNER JOIN NatItemFamily NIF  ON NIC.NatFamilyID = NIF.NatFamilyID
		INNER JOIN Store S  ON	PBDD.Store_No = S.Store_No
		INNER JOIN #ECommerceStores ECOM  ON S.BusinessUnit_ID = ECOM.BusinessUnit_ID 
		INNER JOIN 
		(SELECT
			StoreItemVendor.Store_No,
			StoreItemVendor.Item_Key,
			StoreItemVendor.Vendor_ID,
			Vendor.Vendor_Key,
			StoreItemVendor.DiscontinueItem
			FROM Vendor   
			INNER JOIN StoreItemVendor  ON Vendor.Vendor_ID = StoreItemVendor.Vendor_ID) SIV
		ON	PBDD.Vendor_Key = SIV.Vendor_Key AND
			PBDD.Item_Key = SIV.Item_Key AND
			PBDD.Store_No = SIV.Store_No
		INNER JOIN 
		(SELECT DISTINCT
		PriceBatchDetail.PriceBatchHeaderID,
		PriceBatchDetail.Item_Key,
		PriceBatchDetail.Store_No,
		PriceChgType.PriceChgTypeDesc
		FROM PriceBatchDetail  
		INNER JOIN PriceChgType  ON	PriceBatchDetail.PriceChgTypeID = PriceChgType.PriceChgTypeID) PBD
		ON	PBDD.PriceBatchHeaderID = PBD.PriceBatchHeaderID AND 
			PBDD.Item_Key = PBD.Item_Key AND
			PBDD.Store_No = PBD.Store_No
		LEFT JOIN ItemAttribute IA   ON I.Item_Key = IA.Item_Key
		INNER JOIN StoreItem SI  ON PBDD.Store_No = SI.Store_No AND PBDD.Item_Key = SI.Item_Key
		ORDER BY PBDD.Item_Key, PBDD.Identifier

		-- IsScaleItem should indicate that an item shall be weight at POS, not scale. 
		UPDATE PriceBatchDenorm
			SET IsScaleItem = Sold_By_Weight
		
		INSERT INTO PriceBatchDenorm
		(
			InsertDate,
			Item_Key,
			Item_Description,
			Identifier,
			Sign_Description,
			POS_Description,
			BusinessUnit_ID,
			ECommerce,
			Organic,
			Brand_Name,
			CaseSize,
			Package_Unit_Abbr,
			Price_Change,
			Item_Change,
			On_Sale,
			IsScaleItem,
			ClassID,
			TaxClassID,
			Price,
			Sale_Price,
			Sale_Start_Date,
			PriceChgTypeDesc,
			Sale_End_Date,
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
			NatCatID,
			NatFamilyID,
			Brand_ID
		)
		SELECT -- Add this query to handle attributes that will not trigger a batchable POS Push
			GETDATE() AS InsertDate,
			I.Item_Key,
			I.Item_Description,
			II.Identifier,
			I.Sign_Description,
			I.POS_Description,
			Store.BusinessUnit_ID,
			SI.ECommerce AS ECommerce,
			I.Organic,
			IB.Brand_Name,
			I.Package_Desc2 As CaseSize,
			ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '''') AS Package_Unit_Abbr,
			CASE WHEN ISNULL(P.PriceChgTypeID, P.PriceChgTypeID) IS NOT NULL THEN 1 ELSE 0 END AS Price_Change, 
			0 AS Item_Change,
			PCT.On_Sale, 
			CASE WHEN ISNULL(ISNULL(PU_Override.Weight_Unit, PU.Weight_Unit),0) = 1 AND dbo.fn_IsScaleItem(II.Identifier) = 0 THEN 1 ELSE 0 END AS IsScaleItem,
			I.ClassID,
			I.TaxClassID,
			P.Price,
			P.Sale_Price,
			P.Sale_Start_Date,
			PCT.PriceChgTypeDesc,
			P.Sale_End_Date,
			SIV.DiscontinueItem,
			IA.Check_Box_1,
			IA.Check_Box_2,
			IA.Check_Box_3,
			IA.Check_Box_4,
			IA.Check_Box_5,
			IA.Check_Box_6,
			IA.Check_Box_7,
			IA.Check_Box_8,
			IA.Check_Box_9,
			IA.Check_Box_10,
			IA.Check_Box_11,
			IA.Check_Box_12,
			IA.Check_Box_13,
			IA.Check_Box_14,
			IA.Check_Box_15,
			IA.Check_Box_16,
			IA.Check_Box_17,
			IA.Check_Box_18,
			IA.Check_Box_19,
			IA.Check_Box_20,
			IA.Text_1,
			IA.Text_2,
			IA.Text_3,
			IA.Text_4,
			IA.Text_5,
			IA.Text_6,
			IA.Text_7,
			IA.Text_8,
			IA.Text_9,
			IA.Text_10,
			I.Remove_Item AS IsDeleted,
			SI.Authorized AS IsAuthorized,
			NIC.NatCatID,
			NIF.NatFamilyID,
			I.Brand_ID
		FROM dbo.Price P   
		INNER JOIN dbo.Item I  ON I.Item_Key = P.Item_Key
		INNER JOIN NatItemClass NIS  ON I.ClassID = NIS.ClassID
		INNER JOIN NatItemCat NIC  ON NIS.NatCatID = NIC.NatCatID
		INNER JOIN NatItemFamily NIF  ON NIC.NatFamilyID = NIF.NatFamilyID
		INNER JOIN dbo.StoreItem SI  ON SI.Store_No = P.Store_No AND SI.Item_Key = P.Item_Key 
		INNER JOIN dbo.StoreItemVendor SIV  ON SIV.Item_Key = I.Item_Key AND SIV.Store_No = P.Store_No
		INNER JOIN dbo.ItemIdentifier II  ON II.Item_Key = P.Item_Key 
		INNER JOIN dbo.Store  ON Store.Store_No = P.Store_No
		INNER JOIN PriceChgType PCT  ON P.PriceChgTypeId = PCT.PriceChgTypeID 
		LEFT JOIN dbo.ItemOverride [IO]  ON [IO].Item_Key = P.Item_Key AND [IO].StoreJurisdictionID = Store.StoreJurisdictionID AND @UseRegionalScaleFile = 0 AND @UseStoreJurisdictions = 1
		LEFT JOIN dbo.ItemBrand IB  ON IB.Brand_ID = ISNULL(IO.Brand_ID, I.Brand_ID)
		LEFT JOIN dbo.ItemUnit PU_Override  ON PU_Override.Unit_ID = IO.Package_Unit_ID
		LEFT JOIN dbo.ItemUnit PU  ON PU.Unit_ID = I.Package_Unit_ID
		LEFT JOIN dbo.ItemAttribute IA  ON I.Item_Key = IA.Item_Key 
		WHERE 
			(Mega_Store = 1 OR WFM_Store = 1) AND
			I.LastModifiedDate BETWEEN @Date AND DATEADD(d,1,@Date)
			AND I.Deleted_Item = 0 AND I.Remove_Item = 0 --JAMALI: removed this line from inner join and put it here
			AND SI.Authorized = 1 --JAMALI: removed this from inner join and put it here
			AND SIV.PrimaryVendor = 1 
			AND I.Item_Key NOT IN 
				(SELECT Item_Key 
				FROM PriceBatchDetail PBD  
				WHERE PBD.StartDate = @Date)
		UNION 
		SELECT -- Add this query to handle deleted item
			GETDATE() AS InsertDate,
			I.Item_Key,
			I.Item_Description,
			II.Identifier,
			I.Sign_Description,
			I.POS_Description,
			Store.BusinessUnit_ID,
			SI.ECommerce AS ECommerce,
			I.Organic,
			IB.Brand_Name,
			I.Package_Desc2 As CaseSize,
			ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '''') AS Package_Unit_Abbr,
			CASE WHEN ISNULL(P.PriceChgTypeID, P.PriceChgTypeID) IS NOT NULL THEN 1 ELSE 0 END AS Price_Change, 
			CASE WHEN PBD.ItemChgTypeID IS NOT NULL THEN 1 ELSE 0 END AS Item_Change,
			PCT.On_Sale, 
			CASE WHEN ISNULL(ISNULL(PU_Override.Weight_Unit, PU.Weight_Unit),0) = 1 AND dbo.fn_IsScaleItem(II.Identifier) = 0 THEN 1 ELSE 0 END AS IsScaleItem,
			I.ClassID,
			I.TaxClassID,
			P.Price,
			P.Sale_Price,
			P.Sale_Start_Date,
			PCT.PriceChgTypeDesc,
			P.Sale_End_Date,
			SIV.DiscontinueItem,
			IA.Check_Box_1,
			IA.Check_Box_2,
			IA.Check_Box_3,
			IA.Check_Box_4,
			IA.Check_Box_5,
			IA.Check_Box_6,
			IA.Check_Box_7,
			IA.Check_Box_8,
			IA.Check_Box_9,
			IA.Check_Box_10,
			IA.Check_Box_11,
			IA.Check_Box_12,
			IA.Check_Box_13,
			IA.Check_Box_14,
			IA.Check_Box_15,
			IA.Check_Box_16,
			IA.Check_Box_17,
			IA.Check_Box_18,
			IA.Check_Box_19,
			IA.Check_Box_20,
			IA.Text_1,
			IA.Text_2,
			IA.Text_3,
			IA.Text_4,
			IA.Text_5,
			IA.Text_6,
			IA.Text_7,
			IA.Text_8,
			IA.Text_9,
			IA.Text_10,
			I.Remove_Item AS IsDeleted,
			1 AS IsAuthorized,
			NIC.NatCatID,
			NIF.NatFamilyID,
			I.Brand_ID
		FROM dbo.Price P   
		INNER JOIN PriceBatchDetail PBD  ON P.Item_Key = PBD.Item_Key AND P.Store_No = PBD.Store_No 
		LEFT JOIN PriceBatchHeader PBH  ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID 
		--the Deleted_item and Remove_Item is already present in the where clause
		INNER JOIN dbo.Item I  ON (I.Item_Key = P.Item_Key) --AND (I.Deleted_Item = 1 OR I.Remove_Item = 1)
		INNER JOIN NatItemClass NIS  ON I.ClassID = NIS.ClassID
		INNER JOIN NatItemCat NIC  ON NIS.NatCatID = NIC.NatCatID
		INNER JOIN NatItemFamily NIF  ON NIC.NatFamilyID = NIF.NatFamilyID
		INNER JOIN dbo.StoreItem SI  ON SI.Store_No = P.Store_No AND SI.Item_Key = P.Item_Key
		INNER JOIN dbo.StoreItemVendor SIV  ON SIV.Store_No = P.Store_No AND SIV.Item_Key = I.Item_Key
		INNER JOIN dbo.ItemIdentifier II   ON II.Item_Key = P.Item_Key 
		INNER JOIN dbo.Store  ON Store.Store_No = P.Store_No
		INNER JOIN PriceChgType PCT  ON P.PriceChgTypeId = PCT.PriceChgTypeID 
		LEFT JOIN dbo.ItemOverride [IO]  ON [IO].Item_Key = P.Item_Key AND [IO].StoreJurisdictionID = Store.StoreJurisdictionID
				AND @UseRegionalScaleFile = 0 AND @UseStoreJurisdictions = 1
		LEFT JOIN dbo.ItemBrand IB  ON IB.Brand_ID = ISNULL(IO.Brand_ID, I.Brand_ID)
		LEFT JOIN dbo.ItemUnit PU_Override  ON PU_Override.Unit_ID = IO.Package_Unit_ID
		LEFT JOIN dbo.ItemUnit PU  ON PU.Unit_ID = I.Package_Unit_ID
		LEFT JOIN dbo.ItemAttribute IA  ON I.Item_Key = IA.Item_Key 
		WHERE 
			(Mega_Store = 1 OR WFM_Store = 1) AND
			(CONVERT(DATE, PBD.StartDate) = @Date AND
			PBH.PriceBatchHeaderID IS NOT NULL 
			AND PBD.ItemChgTypeID = 3 
			AND (I.Remove_Item = 1 OR I.Deleted_Item = 1))
			AND SI.Authorized = 1 --JAMALI: Moved this from the inner join
			AND SIV.PrimaryVendor = 1 --JAMALI: Moved this from the inner join
			
		INSERT INTO PriceBatchDenorm
		(
			InsertDate,
			Item_Key,
			Item_Description,
			Identifier,
			Sign_Description,
			POS_Description,
			BusinessUnit_ID,
			ECommerce,
			Organic,
			Brand_Name,
			CaseSize,
			Package_Unit_Abbr,
			Price_Change,
			Item_Change,
			On_Sale,
			IsScaleItem,
			ClassID,
			TaxClassID,
			Price,
			Sale_Price,
			Sale_Start_Date,
			PriceChgTypeDesc,
			Sale_End_Date,
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
			NatCatID,
			NatFamilyID,
			Brand_ID,
			Package_Desc2
		)
		SELECT -- This query handles Authorize / De-authorize, ECommerce, and Discontinued item
			GETDATE() AS InsertDate,
			I.Item_Key,
			I.Item_Description,
			II.Identifier,
			I.Sign_Description,
			I.POS_Description,
			Store.BusinessUnit_ID,
			SI.ECommerce AS ECommerce,
			I.Organic,
			IB.Brand_Name,
			ISNULL(PBDENORM.CaseSize, I.Package_Desc2) AS CaseSize,
			ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '''') AS Package_Unit_Abbr,
			CASE WHEN ISNULL(P.PriceChgTypeID, P.PriceChgTypeID) IS NOT NULL THEN 1 ELSE 0 END AS Price_Change,
			1 AS Item_Change,
			PCT.On_Sale, 
			CASE WHEN ISNULL(ISNULL(PU_Override.Weight_Unit, PU.Weight_Unit),0) = 1 AND dbo.fn_IsScaleItem(II.Identifier) = 0 THEN 1 ELSE 0 END AS IsScaleItem,
			I.ClassID,
			I.TaxClassID,
			P.Price,
			CASE WHEN PCT.On_Sale = 0 THEN PBDENORM.Sale_Price ELSE P.Sale_Price END AS Sale_Price,
			CASE WHEN PCT.On_Sale = 0 THEN PBDENORM.Sale_Start_Date ELSE P.Sale_Start_Date END AS Sale_Start_Date,
			PCT.PriceChgTypeDesc,
			CASE WHEN PCT.On_Sale = 0 THEN PBDENORM.Sale_End_Date ELSE P.Sale_End_Date END AS Sale_End_Date,
			SIV.DiscontinueItem,
			IA.Check_Box_1,
			IA.Check_Box_2,
			IA.Check_Box_3,
			IA.Check_Box_4,
			IA.Check_Box_5,
			IA.Check_Box_6,
			IA.Check_Box_7,
			IA.Check_Box_8,
			IA.Check_Box_9,
			IA.Check_Box_10,
			IA.Check_Box_11,
			IA.Check_Box_12,
			IA.Check_Box_13,
			IA.Check_Box_14,
			IA.Check_Box_15,
			IA.Check_Box_16,
			IA.Check_Box_17,
			IA.Check_Box_18,
			IA.Check_Box_19,
			IA.Check_Box_20,
			IA.Text_1,
			IA.Text_2,
			IA.Text_3,
			IA.Text_4,
			IA.Text_5,
			IA.Text_6,
			IA.Text_7,
			IA.Text_8,
			IA.Text_9,
			IA.Text_10,
			I.Remove_Item AS IsDeleted, 
			SI.Authorized AS IsAuthorized,
			NIC.NatCatID,
			NIF.NatFamilyID,
			I.Brand_ID,
			I.Package_Desc2
		FROM dbo.Price P   
		INNER JOIN #TempDenorm PBD  ON PBD.Item_Key = P.Item_Key AND PBD.Store_No = P.Store_No 
		INNER JOIN dbo.Item I   ON (I.Item_Key = P.Item_Key) 
		INNER JOIN NatItemClass NIS  ON I.ClassID = NIS.ClassID
		INNER JOIN NatItemCat NIC  ON NIS.NatCatID = NIC.NatCatID
		INNER JOIN NatItemFamily NIF  ON NIC.NatFamilyID = NIF.NatFamilyID
		INNER JOIN dbo.StoreItem SI  ON SI.Store_No = P.Store_No  AND SI.Item_Key = P.Item_Key
		INNER JOIN dbo.StoreItemVendor SIV  ON SIV.Store_No = P.Store_No AND SIV.Item_Key = I.Item_Key 
		INNER JOIN dbo.ItemIdentifier II  ON II.Item_Key = P.Item_Key 
		INNER JOIN dbo.Store  ON Store.Store_No = P.Store_No
		INNER JOIN PriceChgType PCT  ON P.PriceChgTypeId = PCT.PriceChgTypeID 
		LEFT JOIN
			(SELECT DISTINCT 
				PBDE.Item_Key, 
				PBDE.CaseSize,
				PBDE.Sale_Price, 
				PBDE.Sale_Start_Date, 
				PBDE.Sale_End_Date
			 FROM @PriceBatchDetailData PBDE) PBDENORM ON I.Item_Key = PBDENORM.Item_Key
		LEFT JOIN dbo.ItemOverride [IO]  ON [IO].Item_Key = P.Item_Key AND [IO].StoreJurisdictionID = Store.StoreJurisdictionID
				AND @UseRegionalScaleFile = 0 AND @UseStoreJurisdictions = 1
		LEFT JOIN dbo.ItemBrand IB  ON IB.Brand_ID = ISNULL([IO].Brand_ID, I.Brand_ID)
		LEFT JOIN dbo.ItemUnit PU  ON PU.Unit_ID = I.Package_Unit_ID
		LEFT JOIN dbo.ItemUnit PU_Override  ON PU_Override.Unit_ID = IO.Package_Unit_ID
		LEFT JOIN dbo.ItemAttribute  IA ON I.Item_Key = IA.Item_Key 
		WHERE 
			(Mega_Store = 1 OR WFM_Store = 1) AND
			CONVERT(VARCHAR(8), I.Item_Key) + ' ' + CONVERT(VARCHAR(8), Store.Store_No) NOT IN 
			(SELECT DISTINCT CONVERT(VARCHAR(8), PBDE.Item_Key) + ' ' + CONVERT(VARCHAR(8), PBDE.Store_No) FROM @PriceBatchDetailData PBDE) 
			AND I.Deleted_Item = 0 AND I.Remove_Item = 0  --Jamali: moved them from the inner join
			AND SIV.PrimaryVendor = 1 --Jamali: Moved it here from the inner join
		
		DROP TABLE #ECommerceStores
		DROP TABLE #TempDenorm

		COMMIT TRANSACTION PopulateDenorm
		
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION PopulateDenorm
	END CATCH
		
END

GO