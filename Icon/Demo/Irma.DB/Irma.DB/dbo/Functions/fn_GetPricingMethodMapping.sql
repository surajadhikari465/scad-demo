CREATE FUNCTION [dbo].[fn_GetPricingMethodMapping] (
    @Store_No int,
    @PricingMethod_Key int )
RETURNS int
AS
BEGIN
    DECLARE @return int
	DECLARE @POSFileWriterKey int

	SET @POSFileWriterKey = (SELECT DISTINCT StorePOSConfig.POSFileWriterKey 
			FROM StorePOSConfig, POSWriter 
			WHERE Store_No=@Store_No AND 
			FileWriterType='POS')
	
    IF @POSFileWriterKey IS NOT NULL
        SELECT @return = PricingMethod_ID FROM POSWriterPricingMethodId 
                      WHERE POSFileWriterKey = @POSFileWriterKey AND PricingMethod_Key = @PricingMethod_Key
    ELSE
        SELECT @return = @PricingMethod_Key
        
    IF @return IS NULL
		SET @return = @PricingMethod_Key
    
    RETURN @return
END