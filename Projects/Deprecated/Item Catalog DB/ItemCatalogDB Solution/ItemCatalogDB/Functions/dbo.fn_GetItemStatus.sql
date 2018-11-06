/****** Object:  UserDefinedFunction [dbo].[fn_CheckItemStatus_Testing]    Script Date: 04/28/2008 09:42:09 ******/
/****** Checks the status of an existing Item for Active, Deleted, Discontinued or Not Available ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER FUNCTION [dbo].[fn_GetItemStatus]
    (@item_key int)
RETURNS varchar (2)

AS

-- ****************************************************************************************************************
-- Procedure: fn_GetItemStatus()
--    Author: unknown
--      Date: unknown
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-04-24	KM		12060	Implement store-level discontinue logic;
-- ****************************************************************************************************************

BEGIN
	DECLARE @result varchar (2) 
		
	SELECT @result = 
		CASE 
			WHEN Deleted_Item  = 1										THEN 'D'
			WHEN dbo.fn_GetDiscontinueStatus(@item_key, null, null) = 1 THEN 'S'
			WHEN Not_Available = 1										THEN 'N'	
			ELSE 'A'
		END
	FROM 
		Item
	WHERE
		Item.Item_Key = @item_key
	
    RETURN @result
END
GO