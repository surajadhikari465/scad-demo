IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_GetGLSuppliesAccountNumber]') AND xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[fn_GetGLSuppliesAccountNumber]
GO
CREATE FUNCTION [dbo].[fn_GetGLSuppliesAccountNumber]
    (@OrderHeader_ID	int)
RETURNS int
AS
-- ********************************************************************************
-- Function: fn_GetGLSuppliesAccountNumber
--           Returns the GL Supplies Account number of the items of a Supplies PO

-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 11/14/2012	FA		7548	Initial version
-- ********************************************************************************
BEGIN

	DECLARE @GLAccountNumber int
	
	SELECT 
		TOP 1 @GLAccountNumber = st.GLSuppliesAcct
	FROM		
		OrderItem				(nolock)	oi 
		INNER JOIN	Item		(nolock)	i	ON i.Item_Key	= oi.Item_Key
		INNER JOIN	SubTeam		(nolock)	st	ON i.SubTeam_No = st.SubTeam_No
	WHERE 
		oi.OrderHeader_ID = @OrderHeader_ID
	
    RETURN @GLAccountNumber
END
GO