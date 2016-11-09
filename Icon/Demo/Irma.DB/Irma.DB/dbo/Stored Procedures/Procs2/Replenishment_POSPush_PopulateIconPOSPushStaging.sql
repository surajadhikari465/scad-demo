
CREATE PROCEDURE dbo.Replenishment_POSPush_PopulateIconPOSPushStaging
	(
	@IconPOSPushStaging	dbo.IconPOSPushStagingType READONLY
	)
AS

-- **************************************************************************
-- Procedure: Replenishment_POSPush_PopulateIconPOSPushStaging
--    Author: Denis Ng
--      Date: 06/12/2014
--
-- Description:
-- This procedure will populate IconPOSPushStaging table with POS Push data
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 05/19/2014	DN   			15056	Created
-- 07/03/2014	DN				15281	Added logic to handle multiple / sale multiple
-- 07/15/2014	DN				15316	Within a row, if any of the Sale_Price, Sale_Start_Date, Sale_End_Date 
--										column is NULL, then all three columns will be NULL.
-- 07/15/2014	DN				15287	Added new column LinkCode_ItemIdentifier
-- 07/15/2014	DN				15314	Added new column POSTare
-- **************************************************************************

DECLARE @DuplicateRows		INT = 0
DECLARE @ConvertMultiple	BIT = 0

SELECT @ConvertMultiple = acv.Value
		FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
		ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca
		ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack
		ON acv.KeyID = ack.KeyID 
		WHERE aca.Name = 'POS PUSH JOB' AND
		ack.Name = 'ConvertMultiple' and
		SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

BEGIN

	INSERT INTO IConPOSPushStaging
	(PriceBatchHeaderID,
	Store_No,
	Item_Key,
	Identifier,
	ChangeType,
	InsertDate,
	RetailSize,
	RetailUOM,
	TMDiscountEligible,
	Case_Discount,
	AgeCode,
	Recall_Flag,
	Restricted_Hours,
	Sold_By_Weight,
	ScaleForcedTare,
	Quantity_Required,
	Price_Required,
	QtyProhibit,
	VisualVerify,
	RestrictSale,
	Price,
	Multiple,
	SaleMultiple,
	Sale_Price,
	Sale_Start_Date,
	Sale_End_Date,
	LinkCode_ItemIdentifier,
	POSTare)
	SELECT DISTINCT
	PriceBatchHeaderID,
	Store_No,
	Item_Key,
	Identifier,
	ChangeType,
	InsertDate,
	RetailSize,
	RetailUOM,
	TMDiscountEligible,
	Case_Discount,
	AgeCode,
	Recall_Flag,
	Restricted_Hours,
	Sold_By_Weight,
	ScaleForcedTare,
	Quantity_Required,
	Price_Required,
	QtyProhibit,
	VisualVerify,
	RestrictSale,
	CASE WHEN (@ConvertMultiple = 1 AND Multiple > 1) THEN ROUND((Price / Multiple),2,1) ELSE Price END AS Price,
	CASE WHEN (@ConvertMultiple = 1 AND Multiple > 1) THEN 1 ELSE Multiple END AS Multiple,
	CASE WHEN (@ConvertMultiple = 1 AND Sale_Multiple > 1) THEN 1 ELSE Sale_Multiple END AS Sale_Multiple,
	CASE WHEN (@ConvertMultiple = 1 AND Sale_Multiple > 1) THEN ROUND((Sale_Price / Sale_Multiple),2,1) ELSE Sale_Price END AS Sale_Price,
	Sale_Start_Date,
	Sale_End_Date,
	LinkCode_ItemIdentifier,
	POSTare
	FROM @IconPOSPushStaging;

	SELECT @DuplicateRows = COUNT(*)
	FROM 
	(SELECT Store_No, 
	Item_Key, 
	Identifier, 
	ChangeType, 
	InsertDate 
	FROM IConPOSPushStaging IPS (NOLOCK)
	GROUP BY Store_No, Item_Key, Identifier, ChangeType, InsertDate
	HAVING COUNT(Item_Key) > 1) DupItems;

	IF @DuplicateRows > 0
		BEGIN
		-- Update duplicate entries with current ForceTare values
		MERGE IConPOSPushStaging IPS2
		USING (SELECT MaxItemScale.*, 
		ITS2.ForceTare 
		FROM ItemScale ITS2 (NOLOCK) 
		INNER JOIN (SELECT MAX(ITS.ItemScale_ID) AS ItemScale_ID,
		Dupitems.Identifier, 
		ITS.Item_Key
		FROM ItemScale ITS (NOLOCK)
		INNER JOIN 
		(SELECT Store_No, 
		Item_Key, 
		Identifier, 
		ChangeType, 
		InsertDate 
		FROM IConPOSPushStaging IPS (NOLOCK)
		GROUP BY Store_No, Item_Key, Identifier, ChangeType, InsertDate
		HAVING COUNT(Item_Key) > 1) DupItems -- Find out the duplicate rows
		ON ITS.Item_Key = DupItems.Item_Key
		GROUP BY ITS.item_key, DupItems.Identifier) MaxItemScale -- Get the most current ItemScale record for those duplicate rows
		ON ITS2.ItemScale_ID = MaxItemScale.ItemScale_ID) CurrForceTare -- Get the most current ForceTare value
		ON IPS2.Item_Key = CurrForceTare.Item_Key AND
		   IPS2.Identifier = CurrForceTare.Identifier
		WHEN MATCHED THEN
			UPDATE
				SET ScaleForcedTare = CurrForceTare.ForceTare; -- Update the dupicate rows with the current ForceTare values
		END

	-- Make sure Sale_Price data includes Sale_Start_Date and Sale_End_End.
	-- If not, all there data fields should be NULL.
	UPDATE IconPOSPushStaging
		SET	Sale_Price = NULL,
			Sale_Start_Date = NULL,
			Sale_End_Date = NULL
	WHERE	Sale_Start_Date IS NULL OR
			Sale_End_Date IS NULL OR
			Sale_Price IS NULL
END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [ItemCatalog.dbo.Replenishment_POSPush_PopulateIconPOSPushStaging.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateIconPOSPushStaging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateIconPOSPushStaging] TO [IRSUser]
    AS [dbo];

