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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Conv_IsY] TO [DataMigration]
    AS [dbo];

