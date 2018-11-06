 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemWebQueryDetailShort]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemWebQueryDetailShort]
GO

CREATE Procedure dbo.ItemWebQueryDetailShort
	@item_key int,
	@StoreJurisdictionID int
as
	-- **************************************************************************
	-- Procedure: ItemWebQueryDetailShort()
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
		I.Item_Key,
		(SELECT TOP 1 Identifier FROM ItemIdentifier ii WHERE ii.Item_Key = i.Item_Key ORDER BY Default_Identifier DESC) AS Identifier,
		ISNULL(OV.Package_Desc2, I.Package_Desc2) AS Package_Desc2, -- Size
		ISNULL(OV.Item_Description, I.Item_Description) AS Item_Description,
		ISNULL(OVU.Unit_Name, IU.Unit_Name) AS Unit_Name
	FROM
		Item (nolock) I
		INNER JOIN ItemUnit (nolock) IU ON I.Package_Unit_ID = IU.Unit_ID
		LEFT OUTER JOIN ItemOverride (nolock) OV ON I.Item_Key = OV.Item_Key AND @StoreJurisdictionID = OV.StoreJurisdictionID
		LEFT OUTER JOIN ItemUnit (nolock) OVU ON OV.Package_Unit_ID = OVU.Unit_ID
	WHERE
		I.Item_Key = @item_key
	
	COMMIT TRAN
END

GO