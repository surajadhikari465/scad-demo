if exists (select * from dbo.sysobjects where id = object_id(N'dbo.EIM_ValidateVendorUOM') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.EIM_ValidateVendorUOM
GO

CREATE PROCEDURE dbo.EIM_ValidateVendorUOM
    @Item_Key int,
    @StoreList varchar(max),
    @VendorUomId int
    ,
    @ValidationCode int OUTPUT
AS

BEGIN

    SET NOCOUNT ON
           
    Set @ValidationCode = 0

	-- the provided vendor uom is invalid if the item uom of
	-- any of the jurisdictions for any
	IF EXISTS
		(
		SELECT 1
		FROM dbo.fn_ParseStringList(@StoreList, ',') storeList
			JOIN Store (NOLOCK)
				ON Store.Store_No = storeList.Key_Value
			JOIN dbo.EIM_Jurisdiction_ItemView ejiv (NOLOCK)
				ON ejiv.StoreJurisdictionId = Store.StoreJurisdictionId
		WHERE ejiv.Item_Key = @Item_Key 
			AND ejiv.CostedByWeight = 1
			AND ejiv.Retail_Unit_ID <> @VendorUomId
		)
	BEGIN
		SET @ValidationCode = 2
	END
	
    SET NOCOUNT OFF
END

GO