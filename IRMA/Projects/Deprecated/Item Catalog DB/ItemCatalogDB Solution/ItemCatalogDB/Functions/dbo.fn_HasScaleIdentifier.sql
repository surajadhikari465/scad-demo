IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_HasScaleIdentifier]') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[fn_HasScaleIdentifier]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION dbo.fn_HasScaleIdentifier 
(
	@Item_Key int
)
RETURNS BIT
AS
BEGIN
    DECLARE @Result BIT
    
    IF EXISTS (SELECT * FROM ItemIdentifier (nolock) WHERE Item_Key = @Item_Key AND Scale_Identifier = 1)
	    SET @Result = 1
    ELSE
	    SET @Result = 0
	    
	RETURN @Result
END
GO