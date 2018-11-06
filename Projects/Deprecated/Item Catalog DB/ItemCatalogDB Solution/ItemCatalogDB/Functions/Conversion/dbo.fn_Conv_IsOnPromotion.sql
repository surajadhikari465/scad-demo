
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Conv_IsOnPromotion]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Conv_IsOnPromotion]
go
Create FUNCTION [dbo].[fn_Conv_IsOnPromotion]
	(@salprend datetime, @ptype	char(4))
	 RETURNS bit
AS
BEGIN  
	DECLARE @return bit

	if @salprend < getdate() 
		select @return = 0
	else if @salprend is null 
		select @return = 0
	else if @ptype in ('SAL','LIN','EDV','CMP','CRD','ISS','TPR')
		select @return = 1
	else
		select @return = 0
		
	RETURN @return
END
go
 