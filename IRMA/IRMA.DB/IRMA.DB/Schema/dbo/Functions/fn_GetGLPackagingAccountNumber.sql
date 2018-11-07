CREATE FUNCTION [dbo].[fn_GetGLPackagingAccountNumber]
    (@OrderHeader_ID	int)
RETURNS int
AS
-- ********************************************************************************
-- Function: fn_GetGLPackagingAccountNumber
--           Returns the GL Packaging Account number of the items of a packaging PO

-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 11/14/2012	FA		7548	Initial version
-- ********************************************************************************
BEGIN

	DECLARE @GLAccountNumber int
	
	SELECT 
		TOP 1 @GLAccountNumber = st.GLPackagingAcct
	FROM		
		OrderItem				(nolock)	oi
		INNER JOIN	Item		(nolock)	i	ON i.Item_Key	= oi.Item_Key
		INNER JOIN	SubTeam		(nolock)	st	ON i.SubTeam_No = st.SubTeam_No
	WHERE 
		oi.OrderHeader_ID = @OrderHeader_ID
	
    RETURN @GLAccountNumber
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetGLPackagingAccountNumber] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetGLPackagingAccountNumber] TO [IRMAReportsRole]
    AS [dbo];

