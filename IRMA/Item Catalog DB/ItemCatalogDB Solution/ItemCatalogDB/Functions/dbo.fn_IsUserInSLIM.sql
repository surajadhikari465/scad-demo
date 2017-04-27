if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_IsUserInSLIM]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_IsUserInSLIM]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Create  FUNCTION dbo.fn_IsUserInSLIM
	(@UserID varchar(13))
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
	
	IF EXISTS(SELECT User_ID FROM SlimAccess WHERE User_ID = @UserID)
		SELECT @return = 1
	ELSE
		SELECT @return = 0
        
	RETURN @return
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 