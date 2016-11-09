
create function dbo.fn_IsRedundantPBDEntry
    (@Item_Key int, @Store_No int)
returns bit
as
begin
	declare @result bit

if exists (select 1				-- Do not create another PBD record for the item if there is already
	FROM PriceBatchDetail PBD	-- an Item Change PBD record not assigned to a batch or assigned to a 
	LEFT JOIN					-- batch in the "Building" status 
		PriceBatchHeader PBH	-- UNLESS the PBD record is for a future dated Off Promo Cost record
		ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
	WHERE PBD.Item_Key = @Item_Key AND PBD.Store_No = @Store_No
		AND ISNULL(PriceBatchStatusID, 0) < 2
		AND PBD.ItemChgTypeID IS NOT NULL
		AND NOT(PBD.ItemChgTypeID = 6 AND PBD.StartDate > GetDate())
		AND Expired = 0)
select @result = 1
else select @result = 0

    return @result
end