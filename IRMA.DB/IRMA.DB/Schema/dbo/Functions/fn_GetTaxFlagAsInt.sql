CREATE FUNCTION dbo.fn_GetTaxFlagAsInt
	(@TaxFlag varchar)
RETURNS int
AS
BEGIN  
DECLARE @return int

IF ISNUMERIC(@TaxFlag) = 1 
	SELECT @return = @TaxFlag 
ELSE
	IF @TaxFlag = 'Y'
		SELECT @return = 1
	ELSE
		SELECT @return = 0
                    
RETURN @return
END