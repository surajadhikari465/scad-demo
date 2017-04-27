SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemsInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemsInfo]
GO


CREATE PROCEDURE dbo.GetItemsInfo
    @Item_Key_list varchar(1000),
    @Vendor_ID int
AS 

--**************************************************************************
-- Function: GetItemsInfo
--    Author: n/a
--      Date: n/a
--
-- Description: This function is called by VendorItems.vb in IRMA client
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 01/04/2013	BS		8755	Coding standards. Updated extension.
--								Updated Discontinue field to account
--								for schema change
--**************************************************************************

BEGIN
    SET NOCOUNT ON
    
	SELECT 
		ii.Identifier,
		i.Item_Description, 
		ISNULL(i.DistSubTeam_No,
		i.SubTeam_No) AS SubTeam_No, 
		i.Brand_ID,
		i.Category_ID, 
		i.Deleted_Item,
		dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, @Vendor_ID) As Discontinue_Item,
		i.WFM_Item,
		i.Not_Available,
		i.Item_Key,
		i.HFM_Item,
		iv.Item_ID,
		ic.Category_Name

	FROM 
		Item											i	(NOLOCK)
		INNER JOIN fn_Parse_List(@Item_Key_list, '|')	il				ON il.Key_Value				= i.Item_Key    
		INNER JOIN ItemIdentifier						ii	(NOLOCK)	ON ii.Item_Key				= i.Item_Key 
																		AND ii.Default_Identifier	= 1
		LEFT JOIN ItemCategory							ic	(NOLOCK)	ON i.Category_ID			= ic.Category_ID
		INNER JOIN ItemVendor							iv	(NOLOCK)	ON iv.Item_Key				= i.Item_Key

	WHERE
		iv.Vendor_ID = isnull(@Vendor_ID, iv.Vendor_ID)    
    
    SET NOCOUNT OFF

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

