
CREATE FUNCTION [dbo].[fn_IsPosPlu]
	(@Identifier varchar(13)
)
RETURNS bit
AS

-- =============================================
-- Author:		Blake Jones
-- Create date: 2016-09-08
-- Description:	Determines whether an Identifier 
--				is a POS PLU or not. An Identifier 
--				is considered a POS PLU if it is
--				less than 7 characters.
-- =============================================

BEGIN  
	DECLARE @return BIT;
	
	IF LEN(@Identifier) < 7
		SET @return = 1;
	ELSE
		SET @return = 0;

	RETURN @return;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsPosPlu] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsPosPlu] TO [IRSUser]
    AS [dbo];

