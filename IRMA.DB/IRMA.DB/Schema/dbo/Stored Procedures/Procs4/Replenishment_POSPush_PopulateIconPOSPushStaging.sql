
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
-- Date       	Init  			TFS   		Comment
-- 05/19/2014	DN   			15056		Created
-- 07/03/2014	DN				15281		Added logic to handle multiple / sale multiple
-- 07/15/2014	DN				15316		Within a row, if any of the Sale_Price, Sale_Start_Date, Sale_End_Date 
--												column is NULL, then all three columns will be NULL.
-- 07/15/2014	DN				15287		Added new column LinkCode_ItemIdentifier
-- 07/15/2014	DN				15314		Added new column POSTare
-- 04/21/2014	DN				19165		Added new column PriceBatchDetailID
-- 08/10/2016	Jamali			PBI17634	Removed the update statement to the staging table
-- **************************************************************************

SET TRANSACTION ISOLATION LEVEL SNAPSHOT


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
	PriceBatchDetailID,
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
	PriceBatchDetailID,
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

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateIconPOSPushStaging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateIconPOSPushStaging] TO [IRSUser]
    AS [dbo];

