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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Conv_IsOnPromotion] TO [DataMigration]
    AS [dbo];

