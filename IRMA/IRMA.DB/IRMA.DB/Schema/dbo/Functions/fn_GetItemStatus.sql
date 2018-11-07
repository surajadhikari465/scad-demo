CREATE FUNCTION [dbo].[fn_GetItemStatus]
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