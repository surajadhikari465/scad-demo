IF EXISTS (SELECT * from dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_IsItemAuthorizedForStore]') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION [dbo].[fn_IsItemAuthorizedForStore]
GO

-- This function checks to see if an item is authorized for ordering and selling at a store in IRMA.
Create  FUNCTION dbo.fn_IsItemAuthorizedForStore (
	@Item_Key int,
	@Store_No int
)
RETURNS bit
AS

BEGIN  
	RETURN ISNULL((SELECT Authorized FROM StoreItem WHERE Item_Key = @Item_Key AND Store_No = @Store_No), 0)
END
GO

 