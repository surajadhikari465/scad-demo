
CREATE PROCEDURE [dbo].[Replenishment_POSPush_GetAllTaxflagData]
(
	@IsScaleZoneData BIT,  -- USED TO LIMIT OUTPUT TO SCALE ITEMS 
	@MaxBatchItems INT,
	@Date DATETIME
)
AS
BEGIN

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL SNAPSHOT

--Section to drop the temp tables
IF OBJECT_ID('tempdb..#Store') IS NOT NULL
BEGIN
	DROP TABLE #Store
END 
IF OBJECT_ID('tempdb..#ItemStoreTable') IS NOT NULL
BEGIN
	DROP TABLE #ItemStoreTable
END 
IF OBJECT_ID('tempdb..#TaxFlags') IS NOT NULL
BEGIN
	DROP TABLE #TaxFlags
END 
IF OBJECT_ID('tempdb..#StoreItems') IS NOT NULL
BEGIN
	DROP TABLE #StoreItems
END 
IF OBJECT_ID('tempdb..#PriceBatchHeaderIds') IS NOT NULL
BEGIN
	DROP TABLE #PriceBatchHeaderIds
END 

--temp tables declaration section
CREATE TABLE #Store
(
	Store_No INT
	, TaxJurisdictionID INT
	, Mega_Store BIT
	, WFM_Store BIT
	, POSFileWriterCode VARCHAR(32)
)

CREATE TABLE #StoreItems
(
	Store_No INT
	, TaxJurisdictionID INT
	, item_key INT
	, TaxClassID INT
)

CREATE TABLE #PriceBatchHeaderIds
(
	PriceBatchHeaderId INT
)

--get all the Non R10 Stores
INSERT INTO #Store
SELECT s.store_no, s.TaxJurisdictionID, Mega_Store, WFM_Store, pos.POSFileWriterCode
FROM Store s
LEFT OUTER JOIN StorePOSConfig spc ON s.Store_No = spc.Store_No
LEFT OUTER JOIN POSWriter pos ON spc.POSFileWriterKey = pos.POSFileWriterKey 
WHERE pos.POSFileWriterCode != 'R10'  --only the data from the non-R10 stores is needed

--Exclude SKUs from the POS/Scale Push?  (TFS 3632)
DECLARE @ExcludeSKUIdentifiers bit
SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)

--Using the regional scale file?
DECLARE @UseRegionalScaleFile bit
SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey='UseRegionalScaleFile')

--get the price batch header id's associated with the batches
INSERT INTO #PriceBatchHeaderIds
SELECT PriceBatchHeaderID FROM dbo.fn_GetPriceBatchHeadersForPushing(@Date, 'false', @MaxBatchItems)
UNION ALL
SELECT PriceBatchHeaderID FROM dbo.fn_GetPriceBatchHeadersForPushing(@Date, 'true', @MaxBatchItems)

--Section to gather data for the Store Items
--Get the store items for the batches
IF EXISTS (SELECT 1 FROM #PriceBatchHeaderIds)
BEGIN
	INSERT INTO #StoreItems
	SELECT  s.Store_No, s.TaxJurisdictionID, i.Item_Key, i.TaxClassID
	FROM #PriceBatchHeaderIds p
	INNER JOIN PriceBatchDetail PBD  ON PBD.PriceBatchHeaderID = p.PriceBatchHeaderID
	INNER JOIN Item i ON i.Item_Key = pbd.Item_Key
	INNER JOIN ItemIdentifier II ON II.Item_Key = i.Item_Key
	INNER JOIN #Store s ON pbd.Store_No = s.Store_No
END

--get the store items for the non batchable items
IF EXISTS (SELECT 1 FROM ItemNonBatchableChanges)
BEGIN
	INSERT INTO #StoreItems
	SELECT s.Store_No, s.TaxJurisdictionID, i.Item_Key, i.TaxClassID
	FROM ItemNonBatchableChanges inbc 
	INNER JOIN StoreItem si ON inbc.Item_Key = si.Item_Key
	INNER JOIN Item i ON si.Item_Key = i.Item_Key
	INNER JOIN #Store s ON si.Store_No = s.Store_No
END

--Get the store items for the POS de-auth records
INSERT INTO #StoreItems
SELECT si.Store_No, s.TaxJurisdictionID, i.Item_Key, i.TaxClassID
FROM #Store s
INNER JOIN  StoreItem si ON s.Store_No = si.Store_No
INNER JOIN Item i ON si.Item_Key = i.Item_Key
WHERE ((@IsScaleZoneData = 0 AND si.POSDeAuth = 1) OR (@IsScaleZoneData = 1 AND si.ScaleDeAuth = 1 AND @UseRegionalScaleFile = 0))

--Get the Store items for the Identifier Deletes, Identifier Adds and Item Refreshs
INSERT INTO #StoreItems
SELECT s.Store_No, s.TaxJurisdictionID, i.Item_Key, i.TaxClassID
FROM  #Store s 
INNER JOIN dbo.Price p ON s.Store_No = p.Store_No
INNER JOIN dbo.Item i ON p.Item_Key = i.Item_Key
INNER JOIN dbo.ItemIdentifier ii  ON i.Item_Key = ii.Item_Key
LEFT OUTER JOIN StoreItem si ON p.Store_No = si.Store_No AND si.Item_Key = p.Item_Key
WHERE (s.Mega_Store = 1 OR s.WFM_Store = 1) AND 
		( 
			--REMOVE IDENIFIER CONDITION
			(ii.Add_Identifier = 0 AND ii.Remove_Identifier = 1)
			--ADD ITEM IDENTIFIER CONDITION
			OR (
					(Deleted_Item = 0 AND Remove_Item = 0 AND Add_Identifier = 1) AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND II.IdentifierType <> 'S'))		
				)
			--ITEM REFRESH CONDITION
			OR (
					(si.Refresh = 1) AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND ii.IdentifierType <> 'S'))		
				)
		)
GROUP BY s.Store_No , s.TaxJurisdictionID, i.Item_Key, i.TaxClassID

--Finally, get the tax flag information for the store items
SELECT st.Store_No, st.Item_Key, TF.TaxFlagKey, TF.TaxFlagValue, TD.TaxPercent, TD.POSID   
FROM #StoreItems st
INNER JOIN Taxflag tf ON tf.TaxClassID = st.TaxClassID AND st.TaxJurisdictionID = tf.TaxJurisdictionID
INNER JOIN TaxDefinition td ON st.TaxJurisdictionID = td.TaxJurisdictionID AND tf.TaxFlagKey = td.TaxFlagKey 
where TF.TaxFlagKey NOT IN	 
	(SELECT TOV.TaxFlagKey 
	 FROM TaxOverride tov 
	 WHERE tov.Item_Key = st.Item_Key AND tov.Store_No = st.Store_No)
GROUP BY st.Store_No, st.Item_Key, tf.TaxFlagKey, tf.TaxFlagValue, TD.TaxPercent, TD.POSID
UNION ALL
-- Read any tax override information for the current item/store combination.
-- Populate the @TaxFlagValues table with the results.
SELECT st.Store_No, st.Item_Key, tov.TaxFlagKey, tov.TaxFlagValue, NULL AS TaxPercent, NULL AS POSID   
FROM  #StoreItems st
INNER JOIN TaxOverride tov on tov.Item_Key = st.Item_Key AND tov.Store_No = st.Store_No
GROUP BY st.Store_No, st.Item_Key, tov.TaxFlagKey, tov.TaxFlagValue
ORDER BY Store_No, Item_Key, TaxFlagKey

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetAllTaxflagData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetAllTaxflagData] TO [IRSUser]
    AS [dbo];

