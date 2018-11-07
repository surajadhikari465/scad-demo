CREATE PROCEDURE [dbo].[InsertCostPromoCodeType]
    @CostPromoCode int,
    @CostPromoDesc varchar(50)
    
AS 
BEGIN
    SET NOCOUNT ON
    
    INSERT INTO CostPromoCodeType (CostPromoCode, CostPromoDesc)
    VALUES (@CostPromoCode, @CostPromoDesc)
    
    SET NOCOUNT OFF
END
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCostPromoCodeType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCostPromoCodeType] TO [IMHARole]
    AS [dbo];

