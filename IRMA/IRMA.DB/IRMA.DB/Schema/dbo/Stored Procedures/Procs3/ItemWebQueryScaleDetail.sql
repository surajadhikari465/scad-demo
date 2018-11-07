CREATE Procedure dbo.ItemWebQueryScaleDetail
	@Item_Key int,
	@StoreJurisdictionID int
as
	-- **************************************************************************
	-- Procedure: ItemWebQueryScaleDetail()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	SELECT 
		SC.Scale_FixedWeight, 
		SC.Scale_ByCount, 
		ISNULL(SCO.Scale_Description1, SC.Scale_Description1) AS Scale_Description1, 
		ISNULL(SCO.Scale_Description2, SC.Scale_Description2) AS Scale_Description2, 
		ISNULL(SCO.Scale_Description3, SC.Scale_Description3) AS Scale_Description3, 
		ISNULL(SCO.Scale_Description4, SC.Scale_Description4) AS Scale_Description4, 
		SC.ShelfLife_Length, 
		SCET.Description, 
		SCET.ExtraText, 
		IU.Unit_Name 
	FROM 
		ItemScale SC (nolock)
		LEFT OUTER JOIN ItemScaleOverride SCO (nolock) ON SC.ITem_Key = SCO.Item_Key AND @StoreJurisdictionID = SCO.StoreJurisdictionID
		LEFT OUTER JOIN ItemUnit IU (nolock) ON ISNULL(SCO.Scale_ScaleUOMUnit_ID, SC.Scale_ScaleUOMUnit_ID) = IU.Unit_ID 
		LEFT OUTER JOIN Scale_ExtraText SCET (nolock) ON ISNULL(SCO.Scale_ExtraText_ID, SC.Scale_ExtraText_ID) = SCET.Scale_ExtraText_ID 
	WHERE 
		SC.Item_Key = @Item_Key

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemWebQueryScaleDetail] TO [IRMASLIMRole]
    AS [dbo];

