CREATE PROCEDURE [dbo].[ItemDeletePendingGetInfo](
    @Item_key int
	,@Identifier varchar(13)
)
AS

/*
	dbo.ItemDeletePendingGetInfo
	The purpose of this procedure is to grab useful information for a user when trying to restore a deleted item that has pending-delete records
	in the PriceBatchDetail table.  In case a pending-delete item cannot be seen in a batching screen, this can provide information about the
	item for investigation.


    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------
    8/18/10             Tom Lux               TFS 13138        Added for v4.0 to support restore-deleted-item.
*/

begin

	-- We base our data-retrieval query off an item key, so if one was not passed, we assume we got an identifier, so we grab its item key.
	if @item_key is null
		-- There should only be one active, non-deleted identifier, so this should not return multiple rows.
		select
			@item_key = Item_Key
		from
			ItemIdentifier 
		where
			Identifier = @identifier
			and Deleted_Identifier = 0

	-- Safeguard against unexpected/odd results by making sure we got an item key from our attempt above.
	if @item_key is not null
	begin
		SELECT
			PriceBatchDetailID = pbd.PriceBatchDetailID
			,Item_Key = PBD.Item_Key
			,Store_No = PBD.Store_No
			,PriceBatchHeaderID = PBD.PriceBatchHeaderID
			,PriceBatchStatus = pbs.PriceBatchStatusDesc
			,BatchDescription = PBH.BatchDescription
			,ApplyDate = PBH.ApplyDate
			,PBHItemChgType = icth.itemchgtypedesc
			,PBDItemChgType = ictd.itemchgtypedesc
			,PBHPriceChgType = pcth.priceChgTypedesc
			,PBDPriceChgType = pctd.priceChgTypedesc
			,PBHStartDate = PBH.StartDate
			,Identifier = PBD.Identifier
			,Insert_Date = PBD.Insert_Date
			,InsertApplication = PBD.InsertApplication
		FROM PriceBatchDetail PBD (nolock)
		left join 
			PriceBatchHeader PBH (nolock)
			ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
		left join 
			itemchgtype icth (nolock)
			on pbh.itemchgtypeid = icth.itemchgtypeid
		left join 
			pricechgtype pcth (nolock) 
			on pbh.pricechgtypeid = pcth.pricechgtypeid
		left join 
			itemchgtype ictd (nolock) 
			on pbh.itemchgtypeid = ictd.itemchgtypeid
		left join 
			pricechgtype pctd (nolock) 
			on pbh.pricechgtypeid = pctd.pricechgtypeid
		left join 
			PriceBatchStatus pbs (nolock) 
			on PBH.PriceBatchStatusID = pbs.PriceBatchStatusID
		WHERE
			PBD.Item_Key = @Item_key
			AND ISNULL(PBD.Expired, 0) = 0
			AND PBD.ItemChgTypeID = 3
			AND ISNULL(pbh.PriceBatchStatusID, 0) < 6 -- If batched, we must restrict pending-delete info to non-pushed batches.
	end
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDeletePendingGetInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDeletePendingGetInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDeletePendingGetInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDeletePendingGetInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDeletePendingGetInfo] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDeletePendingGetInfo] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDeletePendingGetInfo] TO [IRMAPromoRole]
    AS [dbo];

