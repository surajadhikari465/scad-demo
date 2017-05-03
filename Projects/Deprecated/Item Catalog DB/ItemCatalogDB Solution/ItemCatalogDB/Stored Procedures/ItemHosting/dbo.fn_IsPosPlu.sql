IF EXISTS (SELECT * from dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_IsPosPlu]') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION [dbo].[fn_IsPosPlu]
GO

-- =============================================
-- Author:		Blake Jones
-- Create date: 2016-09-08
-- Description:	Determines whether an Identifier 
--				is a POS PLU or not. An Identifier 
--				is considered a POS PLU if it is
--				less than 7 characters.
-- =============================================

CREATE FUNCTION [dbo].[fn_IsPosPlu]
	(@Identifier varchar(13)
)
RETURNS bit
AS

BEGIN  
	DECLARE @return BIT;
	
	IF LEN(@Identifier) < 7
		SET @return = 1;
	ELSE
		SET @return = 0;

	RETURN @return;
END