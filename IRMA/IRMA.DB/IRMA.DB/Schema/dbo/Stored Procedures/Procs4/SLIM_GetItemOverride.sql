CREATE PROCEDURE dbo.SLIM_GetItemOverride
    @Item_Key_List varchar(MAX)
AS
-- **************************************************************************
	-- Procedure: SLIM_GetItemOverride()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
	SET NOCOUNT ON

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	Declare @ItemKeyTable table(Item_Key int)


	IF @Item_Key_List <> ''
    BEGIN
		INSERT INTO @ItemKeyTable
			SELECT Key_value
            FROM fn_Parse_List(@Item_Key_List, '|')
    END

	select io.item_key, io.Item_Description, Package_Desc1, Package_Desc2, iu.Unit_name, sj.StoreJurisdictionDesc, sj.StoreJurisdictionID 
	from ItemOverride io (NOLOCK)
		inner join ItemUnit iu (NOLOCK) on io.Package_unit_ID = iu.Unit_ID
		inner join StoreJurisdiction sj (NOLOCK) on io.StoreJurisdictionID = sj.StoreJurisdictionID
    where io.item_key in (select * from @ItemKeyTable)

	COMMIT TRAN

	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_GetItemOverride] TO [IRMASLIMRole]
    AS [dbo];

