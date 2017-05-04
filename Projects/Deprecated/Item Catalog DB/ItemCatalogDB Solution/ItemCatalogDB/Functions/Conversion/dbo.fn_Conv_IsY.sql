 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Conv_IsY]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Conv_IsY]
go

Create FUNCTION [dbo].[fn_Conv_IsY]
	(@Yes char(1))
	 RETURNS bit
AS
BEGIN  
	DECLARE @return bit
	if @Yes = 'Y'
		select @return = 1
	else
		select @return = 0
		
	RETURN @return
END
go
